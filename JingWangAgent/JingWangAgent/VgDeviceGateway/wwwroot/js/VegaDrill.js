function SetText(txt) {
    var text = this.document.getElementById("text");
    text.innerHTML = txt
}

function CheckInf(id, check) {
    var text = this.document.getElementById(id);
    text.checked = check
}

function handleRequestMaterial(element, text) {
    element.innerText = text;
}

function UpdateInf(id, txt) {
    var text = this.document.getElementById(id);
    text.innerHTML = txt
}

function ChangeBackgroundColor(id,color) {
    var element = this.document.getElementById(id);
    if (element) {
        element.style.background = color;
        element.style.width = "100%";
        element.style.height = "10px";
    }
}

function ChangeBackgroundColorBuffer(id, color) {
    var element = this.document.getElementById(id);
    if (element) {
        element.style.background = color;
        element.style.width = "calc(100% / 8)";
        element.style.padding = "0 25px";
        element.style.height = "10px";
    }
}

 function ChangeBackgroundImage(id, flag) {
    var element = this.document.getElementById(id);
    if (element) {
        element.style.width = "30px";
        element.style.height = "60px";
        if (flag=="1") {
            element.style.background = "url(../images/openSpline.png)";
        } else {
            element.style.background = "url(../images/closeSpline.png)";
        }
        element.style.backgroundSize="100% 100%"
    }
}

function Refresh() {
    setInterval(() => {
        document.location.reload();
    }, 30 * 1000);
}