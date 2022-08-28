const confirmAddToastTrigger = document.getElementById('confirmAddToastBtn')
const confirmAddToast = document.getElementById('confirmAddMessage')
if (confirmAddToastTrigger) {
    confirmAddToastTrigger.addEventListener('click', () => {
        const toast = new bootstrap.Toast(confirmAddToast)
        toast.show()
    })
}

const confirmDeleteToastTrigger = document.getElementById('confirmDeleteToastBtn')
const confirmDeleteToast = document.getElementById('confirmDeleteMessage')
if (confirmDeleteToastTrigger) {
    confirmDeleteToastTrigger.addEventListener('click', () => {
        const toast = new bootstrap.Toast(confirmDeleteToast)
        toast.show()
    })
}