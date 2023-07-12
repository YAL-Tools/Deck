chrome.runtime.onMessage.addListener(function(json, sender) {
	let type = json.type;
	if (type != "lightbox-open" && type != "lightbox-close") return;
	let req = new XMLHttpRequest();
	req.addEventListener("load", console.log);
	req.open("GET", "http://localhost:2023/" + type);
	req.send();
});
console.log("I'm background", new Date());