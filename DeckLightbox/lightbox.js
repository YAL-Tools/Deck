(function() {
	let root =  document.body;
	let scrollElement = document.body.parentElement;
	let imgOrig = document.createElement("img"); // original
	let imgOrigStatus = null; // (false: failed, true: succeeded)
	imgOrig.onerror = (_) => { imgOrigStatus = false };
	imgOrig.onload = (_) => {
		if (imgOrig.src) imgOrigStatus = true;
	}
	//
	let imgFull = document.createElement("img"); // full-sized
	let imgFullStatus = false;
	imgFull.onerror = (_) => { imgFullStatus = false };
	imgFull.onload = (_) => {
		if (imgFull.src) {
			imgFullStatus = true;
			imgOrig.style.visibility = "hidden";
			panUpdate();
		}
	}
	//
	let video = document.createElement("video");
	video.loop = true;
	video.controls = true;
	video.autoplay = true;
	let videoLoaded = false;
	video.oncanplay = (_) => { videoLoaded = true };
	let isVideo = false;
	//
	let panner = document.createElement("div");
	panner.className = "imgxis-panner";
	panner.appendChild(imgOrig);
	panner.appendChild(imgFull);
	panner.appendChild(video);
	let panX = 0, panY = 0, panZ = 0, panM = 1;
	let panWidth = 0, panHeight = 0;
	let panIdle = false; // whether nothing happened to it yet
	let zoomed = false;
	//
	function clearIntervalEx(interval) {
		if (interval != null) {
			clearInterval(interval);
		}
		return null;
	}
	//
	function panUpdate() {
		let pz = (panM >= 1);
		if (pz != zoomed) {
			zoomed = pz;
			let cl = panner.classList;
			if (pz) cl.add("zoomed"); else cl.remove("zoomed");
		}
		let text = (panM*100|0) + "%";
		if (imgFullStatus == null) text += "*";
		panner.setAttribute("zoom", text);
		let tf = `matrix(${panM},0,0,${panM},${-panX|0},${-panY|0})`;
		imgOrig.style.transform = tf;
		imgFull.style.transform = tf;
		video.style.transform = tf;
	}
	//
	function panWheel(e) {
		panIdle = false;
		panner.classList.remove("odd-zoom");
		let d = e.deltaY;
		d = (d < 0 ? 1 : d > 0 ? -1 : 0) * 0.5;
		let zx = e.pageX, zy = e.pageY - scrollElement.scrollTop;
		let prev = panM;
		if (Math.abs(panZ - Math.round(panZ * 2) / 2) > 0.001) {
			panZ = d > 0 ? Math.ceil(panZ * 2) / 2 : Math.floor(panZ * 2) / 2;
		} else panZ = Math.round((panZ + d) * 2) / 2;
		panM = Math.pow(2, panZ);
		let f = panM / prev;
		panX = (zx + panX) * f - zx;
		panY = (zy + panY) * f - zy;
		panUpdate();
		e.preventDefault();
		return false;
	}
	var mouseX = 0, mouseY = 0, mouseDown = false;
	function panMove(e) {
		let lastX = mouseX; mouseX = e.pageX;
		let lastY = mouseY; mouseY = e.pageY;
		if (mouseDown) {
			panX -= (mouseX - lastX);
			panY -= (mouseY - lastY);
			panUpdate();
		}
	}
	function panPress(e) {
        if (e.button == 1) {
            const filter = imgOrig.style.filter == "" ? "invert(1)" : "";
            imgOrig.style.filter = filter;
            imgFull.style.filter = filter;
        }
		panIdle = false;
		panMove(e);
		if (e.target == panner) {
			e.preventDefault();
			setTimeout(() => panHide(), 1);
		} else if (e.which != 3) {
			e.preventDefault();
			mouseDown = true;
		}
	}
	function panRelease(e) {
		panMove(e);
		mouseDown = false;
	}
	function panKeyDown(e) {
		if (e.keyCode == 27/* ESC */) {
			e.preventDefault();
			e.stopPropagation();
			panHide();
			return false;
		}
	}
	panner.addEventListener("mousemove", panMove);
	panner.addEventListener("mousedown", panPress);
	panner.addEventListener("mouseup", panRelease);
	panner.addEventListener("wheel", panWheel);
	//
	function panFit(lw, lh) {
		let iw = window.innerWidth, ih = window.innerHeight;
		panZ = 0;
		if (lw < iw && lh < ih) {
			// zoom in (up to 800%)
			for (let k = 0; k < 3; k++) {
				if (lw * 2 < iw && lh * 2 < ih) {
					panZ += 1; lw *= 2; lh *= 2;
				}
			}
		} else {
			while (lw > iw || lh > ih) { // zoom out until fits
				panZ -= 1; lw /= 2; lh /= 2;
			}
		}
		panM = Math.pow(2, panZ);
		panX = -(iw - lw) / 2;
		panY = -(ih - lh) / 2;
		panWidth = iw;
		panHeight = ih;
		//console.log(iw, ih, lw, lh, panX, panY, panM);
	}
	//
	let checkFullLoad_interval = null;
	function checkFullLoad() {
		if (isVideo) {
			let lw = video.offsetWidth, lh = video.offsetHeight;
			if (!videoLoaded) return;
			checkFullLoad_interval = clearIntervalEx(checkFullLoad_interval);
			panFit(lw, lh);
		} else {
			let lw = imgFull.width, lh = imgFull.height;
			if (lw <= 0 || lh <= 0) return;
			checkFullLoad_interval = clearIntervalEx(checkFullLoad_interval);
			//
			if (imgFullStatus == false) return;
			imgFull.style.visibility = "";
			if (imgFullStatus) {
				// was already used to fit size!
			} else if (/*panIdle*/true) { // it makes sense to rescale to original if idle, but looks odd
				panZ -= Math.log2(Math.max(lw / imgOrig.width, lh / imgOrig.height));
				panM = Math.pow(2, panZ);
				if (Math.abs((panZ * 2) % 1) > 0.001) {
					panner.classList.add("odd-zoom");
				}
			} else panFit(lw, lh);
			imgOrig.width = lw;
			imgOrig.height = lh;
		}
		panUpdate();
	}
	//
	let checkOrigLoad_interval = null;
	function checkOrigLoad() {
		let lw = imgOrig.width, lh = imgOrig.height;
		if (lw <= 0 || lh <= 0) return;
		if (imgOrigStatus == false) return;
		//console.log(lw, lh, img0failed);
		clearInterval(checkOrigLoad_interval); checkOrigLoad_interval = null;
		if (imgFullStatus) {
			panFit(imgFull.width, imgFull.height);
		} else {
			// if the full-sized image already loaded (cached?), we don't want to show original
			imgOrig.style.visibility = "";
			panFit(lw, lh);
		}
		panUpdate();
		if (imgFull.src) {
			checkFullLoad_interval = setInterval(checkFullLoad, 25);
			checkFullLoad();
		}
	}
	//
	let checkWindowSize_interval = null;
	function checkWindowSize() {
		let lastWidth = panWidth; panWidth = window.innerWidth;
		let lastHeight = panHeight; panHeight = window.innerHeight;
		if (panWidth != lastWidth || panHeight != lastHeight) {
			// if the user resizes the window, we want to keep image centered
			panX -= (panWidth - lastWidth) / 2;
			panY -= (panHeight - lastHeight) / 2;
			panUpdate();
		}
	}
	/**
	 * @param {string} fullURL
	 * @param {string} origURL
	 * @param {int} mode (0: image, 1: video)
	 */
	function panShow(fullURL, origURL, mode) {
		chrome.runtime.sendMessage({type:"lightbox-open"});
		isVideo = mode == 1;
		imgFull.style.display = imgOrig.style.display = (mode == 0 ? "" : "none");
		video.style.display = mode == 1 ? "" : "none";
		if (mode == 1) {
			video.src = fullURL;
			videoLoaded = false;
		} else {
			imgOrig.removeAttribute("width");
			imgOrig.removeAttribute("height");
			imgFull.src = fullURL; imgOrigStatus = null;
			imgOrig.src = origURL; imgFullStatus = null;
			imgFull.style.visibility = "hidden";
			imgOrig.style.visibility = "hidden";
		}
		root.appendChild(panner);
		document.addEventListener("keydown", panKeyDown);
		panWidth = window.innerWidth;
		panHeight = window.innerHeight;
		if (mode == 2) return;
		checkWindowSize_interval = setInterval(checkWindowSize, 100);
		if (isVideo) {
			checkFullLoad_interval = setInterval(checkFullLoad, 25);
		} else {
			checkOrigLoad_interval = setInterval(checkOrigLoad, 25);
		}
	}
	function panHide() {
		video.src = "";
        for (let img of [imgOrig, imgFull]) {
            img.src = "";
            img.style.filter = "";
        }
		panner.parentElement.removeChild(panner);
		document.removeEventListener("keydown", panKeyDown);
		clearInterval(checkWindowSize_interval); checkWindowSize_interval = null;
		checkOrigLoad_interval = clearIntervalEx(checkOrigLoad_interval);
		checkFullLoad_interval = clearIntervalEx(checkFullLoad_interval);
		chrome.runtime.sendMessage({type:"lightbox-close"});
	}
	//
	function panGetShow(url, orig, mode) {
		if (mode == null) mode = 0;
		return (e) => {
			e.preventDefault();
			e.stopPropagation();
			panShow(url, orig, mode);
		};
	}
	/** 'url("http://yal.cc/x/_.png")' -> 'http://yal.cc/x/_.png' */
	function getBackgroundUrl(el) {
		let url = el.style.backgroundImage;
		if (url == null) return url;
		return url.slice(4, -1).replace(/"/g, "");
	}
	//
	const attrLbFull = "lb-full";
	const attrLbOrig = "lb-orig";
	const attrLbMode = "lb-mode";
	function onImgClick(e) {
		if (e.button != 0) return;
		if (e.altKey || e.ctrlKey || e.shiftKey || e.metaKey) return;
		let target = e.target;
		let origURL = target.getAttribute(attrLbOrig) ?? target.src;
		let fullURL = target.getAttribute(attrLbFull);
		let mode = target.getAttribute(attrLbMode);
		if (mode) {
			mode = parseInt(mode);
		} else mode = /\.mp4$/.test(fullURL) ? 1 : 0;
		panShow(fullURL, origURL, mode);
		e.preventDefault();
		return false;
	}
	//
	function TwitterChecker() {
		for (let query of [
			`img[draggable="true"]:not([${attrLbFull}])`
		]) for (let img of document.querySelectorAll(query)) {
			let url = img.src;
			let mt = /^https:\/\/pbs.twimg.com\/tweet_video_thumb\/(\w+)/.exec(url);
			if (mt) {
				url = "https://video.twimg.com/tweet_video/" + mt[1] + ".mp4";
				let par = img.parentElement.parentElement.parentElement;
				if (par && par.querySelector('div[role="button"]')) {
					par.setAttribute(attrLbFull, url);
					par.addEventListener("mousedown", onImgClick);
				}
			}
			else if ((mt = /^https:\/\/pbs.twimg.com\/ext_tw_video_thumb\/(\w+)/.exec(url))) {
				// TODO: get someone smart to figure out how fxtwitter and legacy tweetdeck do this
				// https://pbs.twimg.com/ext_tw_video_thumb/1676237527223549954/pu/img/i1OuJJ7HuDmGSCZI?format=jpg&name=small
				// https://video.twimg.com/ext_tw_video/1676237527223549954/pu/vid/1068x720/S7_k33nq4MkDTU-b.mp4
				img.setAttribute(attrLbFull, url);
				continue;
				// (this doesn't work)
				let tweet = img.parentElement;
				while (tweet && tweet.nodeName != "ARTICLE") tweet = tweet.parentElement;
				let time = tweet.querySelector("time");
				if (time && time.parentElement.nodeName == "A") {
					url = time.parentElement.href.replace("://twitter", "://d.fxtwitter");
				}
				img.setAttribute(attrLbMode, "1");
				let par = img.parentElement.parentElement.parentElement;
				if (par && par.querySelector('div[role="button"]')) {
					par.setAttribute(attrLbMode, "1");
					par.setAttribute(attrLbFull, url);
					par.addEventListener("mousedown", onImgClick);
				}
			}
			else {
				url = url.replace(/&name=\w+/g, "&name=4096x4096");
				//url = url.replace(/\?format=jpg/g, "?format=png");
			}
			img.setAttribute(attrLbFull, url);
			img.addEventListener("click", onImgClick, false);
		}
	}
	function CohostChecker() {
		for (let query of [
			`img.object-cover.cursor-pointer:not([${attrLbFull}])`
		]) for (let img of document.querySelectorAll(query)) {
			let url = img.src;
			url = url.replace(/\b(?:width|height)=\w+(?:&|$)/g, "");
			img.setAttribute(attrLbFull, url);
			if (img.nextElementSibling && img.nextElementSibling.classList.contains("cursor-pointer")) {
				// TODO: how does cohost know where to get GIF URL from? Is it all in a closure?
				return;
			}
			img.addEventListener("mousedown", onImgClick, false);
		}
	}
	function MastodonChecker() {
		for (let query of [
			`a.media-gallery__item-thumbnail:not([${attrLbFull}])`
		]) for (let a of document.querySelectorAll(query)) {
			a.setAttribute(attrLbFull, "");
			let img = a.querySelector("img");
			if (!img) continue;
			let url = a.href;
			img.setAttribute(attrLbFull, url);
			img.addEventListener("mousedown", onImgClick, false);
		}
	}
	let checker;
	switch (document.location.host) {
		case "twitter.com": checker = TwitterChecker; break;
		case "cohost.org": checker = CohostChecker; break;
		default: checker = MastodonChecker; break;
	}
	setInterval(checker, 250);
	console.log("lightbox OK!");
})();