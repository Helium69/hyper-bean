function showToast(type, message) {
    const container = document.getElementById("toast-container");

    const toast = document.createElement("div");

    const baseClasses = "px-4 py-3 rounded shadow-md text-sm flex items-center justify-between gap-2 text-white animate-slide-in";
    const bgColor = {
      success: "bg-green-600",
      error: "bg-red-600",
      warning: "bg-yellow-500 text-black"
    };

    toast.className = `${baseClasses} ${bgColor[type] || 'bg-gray-700'}`;

    toast.innerHTML = `
      <span>${message}</span>
      <button onclick="removeToast(this.parentElement)" class="font-bold ${type === 'warning' ? 'text-black' : 'text-white'}">×</button>
    `;

    container.appendChild(toast);

    // Auto fade-out and remove after 3 seconds
    setTimeout(() => {
      toast.classList.remove("animate-slide-in");
      toast.classList.add("animate-fade-out");
      setTimeout(() => toast.remove(), 500); // match fade-out duration
    }, 3000);
  }

  function removeToast(toastElement) {
    toastElement.classList.remove("animate-slide-in");
    toastElement.classList.add("animate-fade-out");
    setTimeout(() => toastElement.remove(), 500);
  }

document.addEventListener("DOMContentLoaded", () => {
    showToast("success", "dsdasdasds")
})