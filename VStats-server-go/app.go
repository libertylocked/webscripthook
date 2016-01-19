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
	router.HandleFunc("/push", handlePostJSON)
	router.HandleFunc("/", handleIndex)

	http.Handle("/", router)
	http.Handle("/skins/", http.StripPrefix("/skins/", http.FileServer(http.Dir("skins"))))
	log.Println("Listening on port " + port)
	http.ListenAndServe(":"+port, nil)
}

func printCredits() {
	fmt.Println("VStats web server by libertylocked")
	fmt.Println("To pull the JSON, use http://localhost:" + port + "/pull")
	fmt.Println("See README for details")
	fmt.Println()
}
