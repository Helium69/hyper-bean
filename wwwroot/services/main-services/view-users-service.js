
    
    const users = [
    {
        id: 1,
        name: "Maximiliano Alejandro de la Cruz Espinoza III",
        birthdate: "1990-03-15",
        username: "maxi_espinoza",
        balance: 1000.25,
        status: "active"
    },
    {
        id: 2,
        name: "Ana Li",
        birthdate: "2002-12-08",
        username: "ana_li",
        balance: 320.00,
        status: "frozen"
    },
    {
        id: 3,
        name: "John Christopher",
        birthdate: "1999-07-22",
        username: "johnny",
        balance: 50.75,
        status: "active"
    }
    ];

    const userList = document.getElementById("user-list");

    function renderUserList() {
    userList.innerHTML = "";

    users.forEach(user => {
        const row = document.createElement("div");
        row.className = "bg-[#2b1008] rounded p-4 shadow text-sm";

        row.innerHTML = `
        <div class="grid grid-cols-2 sm:grid-cols-5 gap-2 sm:gap-4 items-center">
            <div class="break-words font-semibold">👤 ${user.name}</div>
            <div class="break-words">🎂 ${user.birthdate}</div>
            <div class="break-words">🔐 ${user.username}</div>
            <div class="break-words">💰 ₱${user.balance.toFixed(2)}</div>
            <div>
            <button class="${user.status === 'active'
                ? 'bg-green-600 hover:bg-green-500'
                : 'bg-gray-600 hover:bg-gray-500'} text-xs px-3 py-1 rounded toggle-status-btn w-full sm:w-auto">
                ${user.status === 'active' ? 'Active' : 'Frozen'}
            </button>
            </div>
        </div>
        `;

        userList.appendChild(row);
    });
    }

    renderUserList();

    