package main

import (
	"fmt"
	"log"
	"net/http"

	"github.com/gorilla/mux"
)

var port = ""
var router = mux.NewRouter()

func main() {
	port = getPort()
	printCredits()

	router.HandleFunc("/pull", handleGetJSON)
	router.HandleFunc("/getTime", handleGetTimeReceived)
	router.HandleFunc("/pluginconnected", handleGetPluginConnected)
	// for plugin to upload/download data using websocket
	router.HandleFunc("/pushws", handlePluginWS)
	// for browser to send input to plugin
	router.HandleFunc("/input", handlePostInput)
	router.HandleFunc("/dummy", handleGetDummyJSON)
	router.HandleFunc("/", handleIndex)

	http.Handle("/", router)
	http.Handle("/static/", http.StripPrefix("/static/", http.FileServer(http.Dir("static"))))
	http.Handle("/skins/", http.StripPrefix("/skins/", http.FileServer(http.Dir("skins"))))
	log.Println("Listening on port " + port)
	http.ListenAndServe(":"+port, nil)
}

func printCredits() {
	fmt.Println("VStats web server by libertylocked")
	fmt.Println("To pull the JSON, use /pull handle")
	fmt.Println("See README for details")
	fmt.Println()
}
