function success() {
    document.getElementById("status").style.background = "url(/images/complete.png)";
    document.getElementById("status").style.backgroundSize = "cover";
    setTimeout(function () {
        document.getElementById("status").style.background = "url(/images/machine.png)";
        document.getElementById("status").style.backgroundSize = "cover";
    }, 2000);
}
function fail() {
    document.getElementById("status").style.background = "url(/images/running.png)";
    document.getElementById("status").style.backgroundSize = "cover";
    setTimeout(function () {
        document.getElementById("status").style.background = "url(/images/machine.png)";
        document.getElementById("status").style.backgroundSize = "cover";
    }, 2000);
}