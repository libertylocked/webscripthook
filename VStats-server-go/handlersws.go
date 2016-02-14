package main

import (
	"encoding/json"
	"log"
	"net/http"
	"strings"

	"github.com/gorilla/websocket"
	"github.com/satori/go.uuid"
)

type returnPair struct {
	Key   string
	Value interface{}
}

var upgrader = websocket.Upgrader{
	ReadBufferSize:  1024,
	WriteBufferSize: 1024,
}

func handlePluginWS(w http.ResponseWriter, r *http.Request) {
	if pluginConnected {
		// Refuse this connection
		http.Error(w, http.StatusText(401), 401)
		return
	}
	c, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Println("WS:", err)
		return
	}
	defer func() {
		c.Close()
		pluginConnected = false
		log.Println("Plugin disconnected:", r.RemoteAddr)
	}()
	pluginConnected = true
	log.Println("Plugin connected:", r.RemoteAddr)

	for {
		// Read message from plugin
		_, data, err := c.ReadMessage()
		if err != nil {
			log.Println("WS:", err)
			break
		}
		dataString := string(data)
		if strings.HasPrefix(dataString, "RET:") {
			// this is a return value map
			var thisRetPair returnPair
			json.Unmarshal([]byte(dataString[4:]), &thisRetPair)
			uid, _ := uuid.FromString(thisRetPair.Key)
			retChMap[uid] <- thisRetPair.Value
			log.Println("WS: Returned:", uid)
		} else {
			// this is real-time game data
			dataCache = dataString
		}

		// Dequeue one input and send it back
		select {
		case x, ok := <-ch:
			if ok {
				js, err := json.Marshal(x)
				if err != nil {
					log.Println("WS: Failed to marshal input!", err)
					break
				}
				errWrite := c.WriteMessage(websocket.TextMessage, js)
				if errWrite != nil {
					log.Println("WS:", errWrite)
				}
				log.Println("WS: Dequeued:", x.UID)
			} else {
				log.Println("WS: Channel closed!")
			}
		default:
			//fmt.Println("No value ready, moving on.")
		}
	}
}
