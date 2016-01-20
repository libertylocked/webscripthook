package main

import (
	"io"
	"io/ioutil"
	"net/http"
	"time"
)

var dataCache = "NO_DATA"
var timeReceived time.Time

func handleGetJSON(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Access-Control-Allow-Origin", "*")
	io.WriteString(w, dataCache)
}

func handleGetTimeReceived(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Access-Control-Allow-Origin", "*")
	resp := "NO_DATA"
	if !timeReceived.IsZero() {
		resp = timeReceived.Format("15:04:05")
	}
	io.WriteString(w, resp)
}

func handleGetDummyJSON(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Access-Control-Allow-Origin", "*")
	renderTemplate(w, "dummy.json", nil)
}

func handlePostJSON(w http.ResponseWriter, r *http.Request) {
	data := r.PostFormValue("d")
	dataCache = data
	timeReceived = time.Now()
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
