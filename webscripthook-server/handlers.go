package main

import (
	"encoding/json"
	"io"
	"io/ioutil"
	"log"
	"net/http"
	"strconv"
	"time"

	"github.com/satori/go.uuid"
)

type input struct {
	Cmd  string
	Arg  string
	Args []interface{} `json:"Args,omitempty"`
	UID  uuid.UUID
}

var dataCache = "NO_DATA"
var pluginConnected = false
var ch = make(chan input, 50)                       // input channel, used to send inputs from web to game
var retChMap = make(map[uuid.UUID]chan interface{}) // return map, used for plugin return values

func handleGetJSON(w http.ResponseWriter, r *http.Request) {
	// Front-end client gets game data from server
	io.WriteString(w, dataCache)
}

func handleGetPluginConnected(w http.ResponseWriter, r *http.Request) {
	io.WriteString(w, strconv.FormatBool(pluginConnected))
}

func handleGetDummyJSON(w http.ResponseWriter, r *http.Request) {
	data := map[string]string{
		"time": time.Now().Format("15:04:05"),
	}
	renderTemplate(w, "dummy.json", data)
}

func handlePostInput(w http.ResponseWriter, r *http.Request) {
	// Front-end client sends input to game
	decoder := json.NewDecoder(r.Body)
	var t input
	err := decoder.Decode(&t)
	if err != nil {
		log.Println("POST: Error decoding input", err)
		return
	}
	// Attach a UUID to the request input
	t.UID = uuid.NewV4()
	select {
	case ch <- t:
		log.Println("POST: Sent:", t)
		// Now we wait till the plugin sends the return value back
		retChMap[t.UID] = make(chan interface{})
		defer delete(retChMap, t.UID)
		timeout := make(chan bool, 1)
		go func() {
			time.Sleep(1 * time.Second)
			timeout <- true
		}()
		select {
		case retVal := <-retChMap[t.UID]:
			// A return value has arrived!
			seralizedRet, err := json.Marshal(retVal)
			if err != nil {
				log.Println("POST: Return marshal error:", err)
			} else {
				// Return success!
				log.Println("POST: Returned:", string(seralizedRet))
				w.Write(seralizedRet)
			}
		case <-timeout:
			// Return value never arrived (sad face)
			log.Println("POST: Return timeout:", t.UID)
			http.Error(w, http.StatusText(202), 202)
		}
	default:
		// Fails to POST the input because chan is full
		log.Println("POST: Channel full. Discarding value")
	}
}

func handleIndex(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Cache-Control", "no-cache, no-store, must-revalidate") // HTTP 1.1.
	w.Header().Set("Pragma", "no-cache")                                   // HTTP 1.0.
	w.Header().Set("Expires", "0")                                         // Proxies
	files, _ := ioutil.ReadDir("./apps/")
	tmplData := map[string]time.Time{}
	for _, f := range files {
		if f.IsDir() {
			tmplData[f.Name()] = f.ModTime()
		}
	}
	renderTemplate(w, "index.html", tmplData)
	//io.WriteString(w, response)
}
