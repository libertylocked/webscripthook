package main

import (
	"html/template"
	"log"
	"net/http"
	"os"
	"path"
)

func renderTemplateFromDir(w http.ResponseWriter, directoryName string, fileName string, data interface{}) {
	//log.Println("Rendering " + directoryName + "/" + fileName)
	fp := path.Join(directoryName, fileName)

	// Return a 404 if the template doesn't exist
	info, err := os.Stat(fp)
	if err != nil {
		if os.IsNotExist(err) {
			http.Error(w, http.StatusText(404), 404)
			return
		}
	}

	// Return a 404 if the request is for a directory
	if info.IsDir() {
		http.Error(w, http.StatusText(404), 404)
		return
	}

	tmpl, err := template.ParseFiles(fp)
	if err != nil {
		// Log the detailed error
		log.Println(err.Error())
		// Return a generic "Internal Server Error" message
		http.Error(w, http.StatusText(500), 500)
		return
	}

	//if err := tmpl.ExecuteTemplate(w, "_base", data); err != nil {
	if err := tmpl.Execute(w, data); err != nil {
		log.Println(err.Error())
		http.Error(w, http.StatusText(500), 500)
	}
}

func renderTemplate(w http.ResponseWriter, fileName string, data interface{}) {
	renderTemplateFromDir(w, "templates", fileName, data)
}
