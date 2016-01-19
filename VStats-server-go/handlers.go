package main

import (
	"io"
	"net/http"
)

var dataCache = ""

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
