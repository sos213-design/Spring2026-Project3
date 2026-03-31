document.addEventListener("DOMContentLoaded", function () {
    const wheel = document.getElementById("colorWheel");
    const themes = ["light", "dark", "sky", "blue"];

    let currentTheme = localStorage.getItem("theme") || "light";
    document.body.className = currentTheme;

    let index = themes.indexOf(currentTheme);

    if (wheel) {
        wheel.addEventListener("click", function () {
            index = (index + 1) % themes.length;
            document.body.className = themes[index];

            localStorage.setItem("theme", themes[index]);
        });
    }
});




