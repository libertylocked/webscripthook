package main

import (
	"fmt"
	"log"
	"net/http"

	"github.com/gorilla/mux"
)

var port = ""

func main() {
	port = getPort()
	printCredits()

	http.Handle("/", getHandlers())
	http.Handle("/static/", http.StripPrefix("/static/", http.FileServer(http.Dir("static"))))
	http.Handle("/apps/", http.StripPrefix("/apps/", http.FileServer(http.Dir("apps"))))
	log.Println("Listening on port " + port)
	http.ListenAndServe(":"+port, nil)
}

func getHandlers() *mux.Router {
	router := mux.NewRouter()
	router.HandleFunc("/pull", handleGetJSON).Methods("GET")
	router.HandleFunc("/connected", handleGetPluginConnected).Methods("GET")
	// for plugin to upload/download data using websocket
	router.HandleFunc("/pushws", handlePluginWS)
	// for browser to send input to plugin
	router.HandleFunc("/input", handlePostInput).Methods("POST")
	router.HandleFunc("/dummy", handleGetDummyJSON).Methods("GET")
	router.HandleFunc("/", handleIndex).Methods("GET")
	return router
}

func printCredits() {
	fmt.Println("WebScriptHook server by libertylocked")
	fmt.Println("See README for details")
	fmt.Println()
}
