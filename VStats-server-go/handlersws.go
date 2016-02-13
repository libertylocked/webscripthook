package main

import (
	"encoding/json"
	"log"
	"net/http"

	"github.com/gorilla/websocket"
)

var upgrader = websocket.Upgrader{
	ReadBufferSize:  1024,
	WriteBufferSize: 1024,
}

func handlePluginWS(w http.ResponseWriter, r *http.Request) {
	c, err := upgrader.Upgrade(w, r, nil)
	if err != nil {
		log.Println("WS: ", err)
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
		dataCache = string(data)

		// Dequeue one input and send it back
		select {
		case x, ok := <-ch:
			if ok {
				js, err := json.Marshal(x)
				if err != nil {
					log.Println("Failed to marshal input!", err)
					break
				}
				errWrite := c.WriteMessage(websocket.TextMessage, js)
				if errWrite != nil {
					log.Println("WS:", errWrite)
				}
				log.Println("Channel dequeued:", x)
			} else {
				log.Println("Channel closed!")
			}
		default:
			//fmt.Println("No value ready, moving on.")
		}
	}
}
