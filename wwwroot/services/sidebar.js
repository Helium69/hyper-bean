const toggleBtn = document.getElementById('toggleBtn');
const sidebar = document.getElementById('sidebar');
const sidebarLinks = document.querySelectorAll('#sidebarLinks span');

let collapsed = true;

toggleBtn.addEventListener('click', () => {
    collapsed = !collapsed;

    if (!collapsed) {
    sidebar.classList.remove('w-16', 'p-2');
    sidebar.classList.add('w-64', 'p-6');

    // Delay revealing text until animation ends
    setTimeout(() => {
        sidebarLinks.forEach(span => span.classList.remove('hidden'));
        }, 200); // match transition duration
    } else {
    // Hide text immediately before collapse
        sidebarLinks.forEach(span => span.classList.add('hidden'));
        sidebar.classList.add('w-16', 'p-2');
        sidebar.classList.remove('w-64', 'p-6');
    }
});