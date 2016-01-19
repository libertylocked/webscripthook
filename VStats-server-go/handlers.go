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

func handleGetDummyJSON(w http.ResponseWriter, r *http.Request) {
	dummyData := `{"GameTime":"17:28:44","FPS":54.0594521,"RadioStation":"ChannelX","Weather":"ExtraSunny","WantedLevel":0,"PlayerHealth":50,"PlayerArmor":0,"PlayerMoney":167865,"PlayerPos":{"X":1811.52344,"Y":3702.37549,"Z":33.9826851},"ZoneName":"Sandy Shores","StreetName":"Zancudo Ave","WeaponName":"Pistol","WeaponAmmo":2,"WeaponAmmoInClip":0,"WeaponMaxInClip":12,"VehicleName":"Bodhi","VehicleSpeed":26.6951618,"VehicleRPM":0.6957396,"VehicleLicense":"BETTY 32","VehicleType":"Car"}`
	io.WriteString(w, dummyData)
}

func handlePostJSON(w http.ResponseWriter, r *http.Request) {
	//log.Println("Handling POST")
	data := r.PostFormValue("d")
	dataCache = data
	//log.Println(dataCache)
}

func handleIndex(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Cache-Control", "no-cache, no-store, must-revalidate") // HTTP 1.1.
	w.Header().Set("Pragma", "no-cache")                                   // HTTP 1.0.
	w.Header().Set("Expires", "0")                                         // Proxies
	//response := "<h1>Select a skin to start</h1>"
	files, _ := ioutil.ReadDir("./skins/")
	//for _, f := range files {
	//response += `<a href='/skins/` + f.Name() + `/index.html'>` + f.Name() + `</a><br>`
	//}
	renderTemplate(w, "index.html", files)
	//io.WriteString(w, response)
}
