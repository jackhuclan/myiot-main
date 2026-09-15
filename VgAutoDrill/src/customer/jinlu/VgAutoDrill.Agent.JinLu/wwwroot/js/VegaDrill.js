function SetText(txt) {
    var text = this.document.getElementById("text");
    text.innerHTML = txt
}

function handleRequestMaterial(element, text) {
    element.innerText = text;
}
