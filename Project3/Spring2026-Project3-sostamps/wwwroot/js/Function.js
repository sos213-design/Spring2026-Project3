function DModeCycle(){
    const wheel = document.getElementById("colorWheel");
    
    const themes = ["light", "dark", "sky", "blue"];
    let index = 0;
    
    wheel.addEventListener("click", function(){
        document.body.classList.remove(themes[index]);
        index = (index + 1) % themes.length;
        document.body.classList.add(themes[index]);
    });
}
DModeCycle();

