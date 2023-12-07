let tab = function () {
    let tabNav = document.querySelectorAll('.tabs-nav__item'),
        tabContent = document.querySelectorAll('.tab'),
        tabName;

    tabNav.forEach(item => {
        item.addEventListener('click', selectTabNav)
    });


    async function selectTabNav() {
        tabNav.forEach(item => {
            item.classList.remove('is-active');
        });
 
       
        this.classList.add('is-active');
        tabName = this.getAttribute('data-tab-name');
        var chat = document.getElementById(tabName).textContent;
        chatId = chat;
        await change_chat(chatId);
        selectTabContent(tabName);

        //document.getElementById("chatId").setAttribute("id","")*/
        console.log(tabName)
    }

    async function selectTabContent(tabName) {
        tabContent.forEach(item => {
            item.classList.contains(tabName) ? item.classList.add('is-active') : item.classList.remove('is-active');
        })
    }

};


tab();