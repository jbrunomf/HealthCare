    $(document).ready(function () {
        $('#appointmentDate').on("change", function () {
            console.log('Date changed to:', $(this).val());
            var doctorId = $('#doctorId').val();
            var date = $("#appointmentDate").val(); // Data atual (ou você pode permitir o usuário selecionar uma data)
            if (doctorId) {
                $.ajax({
                    url: '/Appointments/GetAvailableSlots', // Endpoint do controller
                    type: 'GET',
                    data: { doctorId: doctorId, date: date },
                    success: function (slots) {
                        var slotsSelect = $('#medicalScheduleId');
                        slotsSelect.empty(); // Limpa os horários anteriores
                        if (slots.length > 0) {
                            slotsSelect.append('<option value="">Select a slot</option>');
                            slots.forEach(function (slot) {
                                slotsSelect.append(`
                                    <option value="${slot.id}">
                                        ${new Date(slot.startTime).toLocaleTimeString()} - ${new Date(slot.endTime).toLocaleTimeString()}
                                    </option>
                                `);
                            });
                        } else {
                            slotsSelect.append('<option value="">No available slots for this professional.</option>');
                        }
                    },
                    error: function () {
                        alert('Failed to load available slots. Please try again.');
                    }
                });
            } else {
                $('#available-slots').html('<option value="">Select a professional to see available slots.</option>');
            }
        });
    });
