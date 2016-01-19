package main

import (
	"io"
	"io/ioutil"
	"net/http"
)

var dataCache = "NO_DATA"

func handleGetJSON(w http.ResponseWriter, r *http.Request) {
	//filebytes, _ := ioutil.ReadFile("vstats.json")
	//w.Write(filebytes)
	io.WriteString(w, dataCache)
}

func handlePostJSON(w http.ResponseWriter, r *http.Request) {
	//log.Println("Handling POST")
	data := r.PostFormValue("d")
	dataCache = data
	//log.Println(dataCache)
}

func handleIndex(w http.ResponseWriter, r *http.Request) {
	//response := "<h1>Select a skin to start</h1>"
	files, _ := ioutil.ReadDir("./skins/")
	//for _, f := range files {
	//response += `<a href='/skins/` + f.Name() + `/index.html'>` + f.Name() + `</a><br>`
	//}
	renderTemplate(w, "index.html", files)
	//io.WriteString(w, response)
}
