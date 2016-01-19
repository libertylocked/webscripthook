package main

import (
	"fmt"
	"io/ioutil"
	"log"

	"gopkg.in/gcfg.v1"
)

func getPort() string {
	log.Println("Reading config")
	filebytes, _ := ioutil.ReadFile("VStats-plugin.ini")
	cfgStr := string(filebytes)
	fmt.Println(cfgStr)
	cfg := struct {
		Core struct {
			HOST string
			PORT string
		}
	}{}
	err := gcfg.ReadStringInto(&cfg, cfgStr)
	if err != nil {
		log.Fatalf("Failed to parse gcfg data: %s", err)
	}
	return cfg.Core.PORT
}
