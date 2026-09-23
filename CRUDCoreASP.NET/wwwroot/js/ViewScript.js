let isEditMode = false;

function openModal(isEdit, id = 0, firstName = '', middleName = '', lastName = '') {
    isEditMode = isEdit;
    const modal = document.getElementById('userModal');

    document.getElementById('userId').value = id;
    document.getElementById('firstName').value = firstName;
    document.getElementById('middleName').value = middleName;
    document.getElementById('lastName').value = lastName;

    if (isEditMode) {
        document.getElementById('modalTitle').innerText = 'Update Account Profile';
        document.getElementById('modalDescription').innerText = 'Modify user information settings below.';
        document.getElementById('submitBtn').innerText = 'Update Record';
    } else {
        document.getElementById('modalTitle').innerText = 'Register New Account';
        document.getElementById('modalDescription').innerText = 'Please fill out the form fields below to create a secure profile.';
        document.getElementById('submitBtn').innerText = 'Save Record';
        document.getElementById('userForm').reset();
    }

    modal.style.display = 'flex';
}

function closeModal() {
    document.getElementById('userModal').style.display = 'none';
}

window.onclick = function (event) {
    const modal = document.getElementById('userModal');
    if (event.target == modal) {
        closeModal();
    }
}

function saveUser(event) {
    event.preventDefault();

    const userData = {
        id: parseInt(document.getElementById('userId').value) || 0,
        FirstName: document.getElementById('firstName').value,
        MiddleName: document.getElementById('middleName').value,
        LastName: document.getElementById('lastName').value
    };

    const url = isEditMode ? '/Home/EditUser' : '/Home/AddUser';

    Swal.fire({
        title: 'Naglo-load...',
        text: 'Ipinapadala ang impormasyon sa server.',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: new URLSearchParams(userData)
    })
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.json();
        })
        .then(data => {
            Swal.fire({
                icon: 'success',
                title: isEditMode ? 'Matagumpay na Na-update!' : 'Matagumpay na Nairehistro!',
                text: 'Ilo-load muli ang pahina para ma-update ang listahan.',
                timer: 2000,
                showConfirmButton: false
            }).then(() => {
                window.location.reload();
            });
        })
        .catch(error => {
            console.error('Error processing record layout request:', error);
            Swal.fire({
                icon: 'error',
                title: 'Ops! May Error',
                text: 'Hindi maiproseso ang iyong request ngayon.',
                confirmButtonColor: '#3085d6'
            });
        });
}

function deleteUser(id) {
    Swal.fire({
        title: 'Sigurado ka ba?',
        text: "Permanenteng mabubura ang account record na ito!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Oo, burahin!',
        cancelButtonText: 'Kanselahin'
    }).then((result) => {
        if (result.isConfirmed) {

            Swal.fire({
                title: 'Binubura...',
                allowOutsideClick: false,
                didOpen: () => { Swal.showLoading(); }
            });

            fetch('/Home/DeleteUser', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: new URLSearchParams({ id: id })
            })
                .then(response => {
                    if (!response.ok) throw new Error('Network response was not ok');
                    return response.json();
                })
                .then(status => {
                    Swal.fire({
                        icon: 'success',
                        title: 'Nabura na!',
                        text: status.statusName || 'Matagumpay na nabura ang record.',
                        timer: 2000,
                        showConfirmButton: false
                    }).then(() => {
                        window.location.reload();
                    });
                })
                .catch(error => {
                    console.error('Deletion transmission error:', error);
                    Swal.fire({
                        icon: 'error',
                        title: 'Ops! May Error',
                        text: 'Hindi nabura ang record.',
                        confirmButtonColor: '#3085d6'
                    });
                });
        }
    });
}
