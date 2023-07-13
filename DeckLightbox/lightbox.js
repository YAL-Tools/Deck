(function() {
	let root =  document.body;
	let scrollElement = document.body.parentElement;
	let imgOrig = document.createElement("img"); // original
	let imgOrigFailed = false;
	imgOrig.onerror = (_) => { imgOrigFailed = true };
	//
	let imgFull = document.createElement("img"); // full-sized
	let imgFullFailed = false;
	imgFull.onerror = (_) => { imgFullFailed = true };
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
	function panUpdate() {
		let pz = (panM >= 1);
		if (pz != zoomed) {
			zoomed = pz;
			let cl = panner.classList;
			if (pz) cl.add("zoomed"); else cl.remove("zoomed");
		}
		panner.setAttribute("zoom", `${panM*100|0}%`);
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
	let panCheckInt2 = null;
	function panCheck2() {
		if (isVideo) {
			let lw = video.offsetWidth, lh = video.offsetHeight;
			if (!videoLoaded) return;
			clearInterval(panCheckInt2); panCheckInt2 = null;
			panFit(lw, lh);
			console.log(lw, lh, panX, panY);
		} else {
			let lw = imgFull.width, lh = imgFull.height;
			if (lw <= 0 || lh <= 0) return;
			clearInterval(panCheckInt2); panCheckInt2 = null;
			//
			if (imgFullFailed) return;
			imgFull.style.visibility = "";
			if (/*panIdle*/true) { // it makes sense to rescale to original if idle, but looks odd
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
	let panCheckInt = null;
	function panCheck() {
		let lw = imgOrig.width, lh = imgOrig.height;
		if (lw <= 0 || lh <= 0) return;
		if (imgOrigFailed) return;
		//console.log(lw, lh, img0failed);
		clearInterval(panCheckInt); panCheckInt = null;
		panFit(lw, lh);
		imgOrig.style.visibility = "";
		panUpdate();
		if (imgFull.src) {
			panCheckInt2 = setInterval(panCheck2, 25);
		}
	}
	//
	var panTickInt = null;
	function panTick() {
		let lastWidth = panWidth; panWidth = window.innerWidth;
		let lastHeight = panHeight; panHeight = window.innerHeight;
		if (panWidth != lastWidth || panHeight != lastHeight) {
			panX -= (panWidth - lastWidth) / 2;
			panY -= (panHeight - lastHeight) / 2;
			panUpdate();
		}
	}
	function panShow(url, orig, mode) {
		chrome.runtime.sendMessage({type:"lightbox-open"});
		isVideo = mode == 1;
		imgFull.style.display = imgOrig.style.display = (mode == 0 ? "" : "none");
		video.style.display = mode == 1 ? "" : "none";
		if (mode == 1) {
			video.src = url;
			videoLoaded = false;
		} else {
			imgOrig.removeAttribute("width");
			imgOrig.removeAttribute("height");
			imgFull.src = url; imgOrigFailed = false;
			imgOrig.src = orig; imgFullFailed = false;
			imgFull.style.visibility = "hidden";
			imgOrig.style.visibility = "hidden";
		}
		root.appendChild(panner);
		document.addEventListener("keydown", panKeyDown);
		panWidth = window.innerWidth;
		panHeight = window.innerHeight;
		if (mode == 2) return;
		panTickInt = setInterval(panTick, 100);
		if (isVideo) {
			panCheckInt2 = setInterval(panCheck2, 25);
		} else {
			panCheckInt = setInterval(panCheck, 25);
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
		clearInterval(panTickInt); panTickInt = null;
		if (panCheckInt != null) { clearInterval(panCheckInt); panCheckInt = null; }
		if (panCheckInt2 != null) { clearInterval(panCheckInt2); panCheckInt2 = null; }
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
	//
	function getBackgroundUrl(el) {
		let url = el.style.backgroundImage;
		if (url == null) return url;
		return url.slice(4, -1).replace(/"/g, "");
	}
	//
	const attrLbSrc = "lb-src";
	function onImgClick(e) {
		if (e.button != 0) return;
		if (e.altKey || e.ctrlKey || e.shiftKey || e.metaKey) return;
		let target = e.target;
		panShow(target.getAttribute(attrLbSrc), target.src, 0);
		e.preventDefault();
		return false;
	}
	//
	function TwitterChecker() {
		for (let query of [
			`img[draggable="true"]:not([${attrLbSrc}])`
		]) for (let img of document.querySelectorAll(query)) {
			let url = img.src;
			url = url.replace(/&name=\w+/g, "&name=4096x4096");
			//url = url.replace(/\?format=jpg/g, "?format=png");
			img.setAttribute(attrLbSrc, url);
			img.addEventListener("click", onImgClick);
		}
	}
	function CohostChecker() {
		for (let query of [
			`img.object-cover.cursor-pointer:not([${attrLbSrc}])`
		]) for (let img of document.querySelectorAll(query)) {
			let url = img.src;
			url = url.replace(/\b(?:width|height)=\w+(?:&|$)/g, "");
			img.setAttribute(attrLbSrc, url);
			img.addEventListener("mousedown", onImgClick, false);
		}
	}
	function MastodonChecker() {
		for (let query of [
			`a.media-gallery__item-thumbnail:not([${attrLbSrc}])`
		]) for (let a of document.querySelectorAll(query)) {
			a.setAttribute(attrLbSrc, "");
			let img = a.querySelector("img");
			if (!img) continue;
			let url = a.href;
			img.setAttribute(attrLbSrc, url);
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