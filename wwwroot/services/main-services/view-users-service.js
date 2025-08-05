document.addEventListener("DOMContentLoaded", async () => {
    await refreshList();
});

document.addEventListener("click", async (e) => {
    if (e.target.classList.contains("status-button")){
        const id = parseInt(e.target.dataset.id);

        const response = await fetch("/user/update-user-status", {
            method : "POST",
            credentials : "include",
            headers : {"Content-Type" : "application/json"},
            body : JSON.stringify({ id : id })
        })

        const responseData = await response.json();

        if (response.status === 200){
            await refreshList();
            return;
        }

        debugger;
    }   
});


async function refreshList(){
    const response = await fetch ("/admin/get-users", {
        method : "GET",
        credentials : "include"
    });

    const responseData = await response.json();

    renderUserList(responseData.data);
}

function renderUserList(users) {
    const userList = document.getElementById("user-list");
    userList.innerHTML = "";

    users.forEach(user => {
        const row = document.createElement("div");
        row.className = "bg-[#2b1008] rounded p-4 shadow text-sm";

        row.innerHTML = `
            <!-- Column Headers -->
            <div class="text-xs text-yellow-300 font-semibold px-4 pb-2 grid items-center gap-4"
                style="grid-template-columns: 160px 120px 140px 110px 80px 80px;">
                <div>Name</div>
                <div>Birth Date</div>
                <div>Username</div>
                <div>Balance</div>
                <div>Sex</div>
                <div>Status</div>
            </div>


            <div class="bg-[#541a08] rounded-xl px-4 py-2 shadow text-sm grid items-center gap-4"
                style="grid-template-columns: 160px 120px 140px 110px 80px 80px;">

                    

                <!-- Profile Picture + Name -->
                <div class="flex items-start gap-2 min-w-0">
                    <img src="${user.url}" alt="Profile"
                        class="w-6 h-6 rounded-full object-cover border border-white mt-0.5 shrink-0">
                    <span class="font-semibold break-words leading-tight">${user.name}</span>
                </div>

                <!-- Birth Date -->
                <div class="truncate">${user.birthDate}</div>

                <!-- Username -->
                <div class="break-words leading-tight">${user.username}</div>


                <!-- Balance -->
                <div class="truncate">₱${user.userBalance}</div>

                <!-- Sex -->
                <div class="truncate">${user.sex}</div>

                <!-- Status Button -->
                <div class="flex justify-end">
                <button data-id="${user.id}" class="status-button ${user.isActive === true
                    ? 'bg-green-600 hover:bg-green-500'
                    : 'bg-gray-600 hover:bg-gray-500'} text-xs px-3 py-1 rounded toggle-status-btn w-full sm:w-auto">
                    ${user.isActive === true ? 'Active' : 'Frozen'}
                </button>
                </div>
            </div>

            `;

        userList.appendChild(row);
    });
}

    

    