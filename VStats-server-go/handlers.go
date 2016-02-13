package main

import (
	"encoding/json"
	"io"
	"io/ioutil"
	"log"
	"net/http"
	"time"
)

type input struct {
	Cmd  string
	Arg  string
	Args []interface{} `json:"Args,omitempty"`
}

var dataCache = "NO_DATA"
var timeReceived time.Time
var pluginConnected = false
var ch = make(chan input, 10) // input channel, used to send inputs from web to game

func handleGetJSON(w http.ResponseWriter, r *http.Request) {
	// Front-end client gets game data from server
	io.WriteString(w, dataCache)
}

func handleGetTimeReceived(w http.ResponseWriter, r *http.Request) {
	resp := "NO_DATA"
	if !timeReceived.IsZero() {
		resp = timeReceived.Format("15:04:05")
	}
	io.WriteString(w, resp)
}

func handleGetDummyJSON(w http.ResponseWriter, r *http.Request) {
	data := map[string]string{
		"time": time.Now().Format("15:04:05"),
	}
	renderTemplate(w, "dummy.json", data)
}

func handlePostInput(w http.ResponseWriter, r *http.Request) {
	decoder := json.NewDecoder(r.Body)
	var t input
	err := decoder.Decode(&t)
	if err != nil {
		log.Println("POST: Error decoding input", err)
		return
	}

	select {
	case ch <- t:
		log.Println("POST: Sent to channel:", t)
	default:
		log.Println("POST: Channel full. Discarding value")
	}
}

func handleIndex(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Cache-Control", "no-cache, no-store, must-revalidate") // HTTP 1.1.
	w.Header().Set("Pragma", "no-cache")                                   // HTTP 1.0.
	w.Header().Set("Expires", "0")                                         // Proxies
	//response := "<h1>Select a skin to start</h1>"
	files, _ := ioutil.ReadDir("./skins/")
	tmplData := map[string]time.Time{}
	for _, f := range files {
		if f.IsDir() {
			tmplData[f.Name()] = f.ModTime()
		}
	}
	renderTemplate(w, "index.html", tmplData)
	//io.WriteString(w, response)
}
