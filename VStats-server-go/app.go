package main

import (
	"log"
	"net/http"

	"github.com/gorilla/mux"
)

var port = ""
var router = mux.NewRouter()

func main() {
	port = getPort()

	router.HandleFunc("/pull", handleGetJSON)
	router.HandleFunc("/push", handlePostJSON)

	http.Handle("/", router)
	log.Println("Listening on port " + port)
	http.ListenAndServe(":"+port, nil)
}
