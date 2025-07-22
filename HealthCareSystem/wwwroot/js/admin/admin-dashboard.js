// dashboard.js - Updated version
let appointmentChart = null;
let appointmentOverviewChart = null;
let patientStatusChart = null;
let currentAppointmentPeriod = 'monthly';
let appointmentData = null;

// Initialize charts when page loads
document.addEventListener('DOMContentLoaded', function () {
    initializeCharts();
    loadAppointmentStatistics();
});

function initializeCharts() {
    // Initialize Appointment Trends Chart
    const appointmentCtx = document.getElementById('appointmentTrendsChart').getContext('2d');
    appointmentChart = new Chart(appointmentCtx, {
        type: 'line',
        data: {
            labels: [],
            datasets: [{
                label: 'Appointments',
                data: [],
                borderColor: '#4e73df',
                backgroundColor: 'rgba(78, 115, 223, 0.1)',
                borderWidth: 2,
                fill: true,
                tension: 0.3
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            },
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    });

    // Initialize Appointment Overview Chart
    const overviewCtx = document.getElementById('appointmentOverviewChart').getContext('2d');
    appointmentOverviewChart = new Chart(overviewCtx, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: 'Confirmed',
                data: [],
                backgroundColor: '#1cc88a',
                borderColor: '#1cc88a',
                borderWidth: 1
            }, {
                label: 'Pending',
                data: [],
                backgroundColor: '#f6c23e',
                borderColor: '#f6c23e',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        stepSize: 1
                    }
                }
            }
        }
    });

    // Initialize Patient Status Chart
    const statusCtx = document.getElementById('patientStatusChart').getContext('2d');
    patientStatusChart = new Chart(statusCtx, {
        type: 'doughnut',
        data: {
            labels: [],
            datasets: [{
                data: [],
                backgroundColor: [],
                borderColor: '#ffffff',
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    });
}

async function loadAppointmentStatistics() {
    try {
        const response = await fetch('/Admin/GetDashboardStatistics');
        if (response.ok) {
            const data = await response.json();

            console.log('=== FULL RECEIVED DATA ===');
            console.log(data);
            console.log('=== STATUS DISTRIBUTION ===');
            console.log(data.statusDistribution);
            console.log('=== APPOINTMENT TRENDS ===');
            console.log(data.appointmentTrends);
            console.log('=== DAILY TRENDS ===');
            console.log(data.dailyAppointmentTrends);
            console.log('=== WEEKLY TRENDS ===');
            console.log(data.weeklyAppointmentTrends);

            // Update statistics cards
            document.getElementById('totalPatients').textContent = data.totalPatients || 0;
            document.getElementById('activePatients').textContent = data.activePatients || 0;
            document.getElementById('totalAppointments').textContent = data.totalAppointments || 0;
            document.getElementById('pendingAppointments').textContent = data.pendingAppointments || 0;

            // Store data globally
            window.appointmentData = data;
            window.currentAppointmentPeriod = 'monthly';

            // Update appointment trends chart - FIX: Check data exists
            if (appointmentChart && data.appointmentTrends && data.appointmentTrends.length > 0) {
                console.log('Updating appointment trends chart...');
                appointmentChart.data.labels = data.appointmentTrends.map(t => t.month);
                appointmentChart.data.datasets[0].data = data.appointmentTrends.map(t => t.count);
                appointmentChart.update();

                // Update period stats
                updatePeriodStats(data.appointmentTrends.map(t => t.count));
            } else {
                console.warn('No appointment trends data available');
            }

            // Update appointment overview chart
            if (appointmentOverviewChart && data.appointmentOverviewData && data.appointmentOverviewData.length > 0) {
                console.log('Updating appointment overview chart...');
                appointmentOverviewChart.data.labels = data.appointmentOverviewData.map(t => t.month);
                appointmentOverviewChart.data.datasets[0].data = data.appointmentOverviewData.map(t => t.confirmed);
                appointmentOverviewChart.data.datasets[1].data = data.appointmentOverviewData.map(t => t.pending);
                appointmentOverviewChart.update();
            }

            // Update patient status chart - FIX: Debug and ensure data
            if (patientStatusChart && data.statusDistribution && data.statusDistribution.length > 0) {
                console.log('Updating patient status chart...');
                console.log('Status data:', data.statusDistribution);

                patientStatusChart.data.labels = data.statusDistribution.map(s => s.status);
                patientStatusChart.data.datasets[0].data = data.statusDistribution.map(s => s.count);
                patientStatusChart.data.datasets[0].backgroundColor = data.statusDistribution.map(s => s.color);
                patientStatusChart.update();

                // Update patient status legend
                updatePatientStatusLegend(data.statusDistribution);
            } else {
                console.warn('No patient status distribution data available');
                console.log('Status distribution:', data.statusDistribution);
            }

            // Update recent patients
            if (data.recentPatients) {
                updateRecentPatientsList(data.recentPatients);
            }

            // Update growth indicators
            updateGrowthIndicators(data);

        } else {
            console.error('Failed to load appointment statistics:', response.status);
        }
    } catch (error) {
        console.error('Error loading appointment statistics:', error);
    }
}

function updateGrowthIndicators(data) {
    const patientGrowth = document.getElementById('patientGrowth');
    const newPatients = document.getElementById('newPatients');
    const confirmedGrowth = document.getElementById('confirmedGrowth');
    const pendingGrowth = document.getElementById('pendingGrowth');

    if (patientGrowth && data.newPatientsThisMonth >= 0) {
        patientGrowth.textContent = `+${data.newPatientsThisMonth} this month`;
        patientGrowth.className = 'text-success';
    }

    if (newPatients && data.newPatientsThisWeek >= 0) {
        newPatients.textContent = `+${data.newPatientsThisWeek} this week`;
        newPatients.className = 'text-info';
    }

    if (confirmedGrowth) {
        confirmedGrowth.textContent = `${data.confirmedAppointments || 0} confirmed`;
        confirmedGrowth.className = 'text-success';
    }

    if (pendingGrowth) {
        pendingGrowth.textContent = `${data.pendingAppointments || 0} pending`;
        pendingGrowth.className = 'text-warning';
    }
}

function updatePeriodStats(data) {
    if (!data || data.length === 0) {
        document.getElementById('totalPeriodAppointments').textContent = 0;
        document.getElementById('averageAppointments').textContent = 0;
        document.getElementById('peakAppointments').textContent = 0;
        return;
    }

    const total = data.reduce((sum, val) => sum + val, 0);
    const average = Math.round(total / data.length);
    const peak = Math.max(...data);

    document.getElementById('totalPeriodAppointments').textContent = total;
    document.getElementById('averageAppointments').textContent = average;
    document.getElementById('peakAppointments').textContent = peak;
}

function updatePatientStatusLegend(statusData) {
    const legendContainer = document.getElementById('patientStatusLegend');
    if (!legendContainer) return;

    legendContainer.innerHTML = '';

    if (!statusData || statusData.length === 0) {
        legendContainer.innerHTML = '<div class="text-center text-muted">No data available</div>';
        return;
    }

    const totalCount = statusData.reduce((sum, s) => sum + s.count, 0);

    statusData.forEach(item => {
        const percentage = totalCount > 0 ? ((item.count / totalCount) * 100).toFixed(1) : 0;
        const legendItem = document.createElement('div');
        legendItem.className = 'd-flex justify-content-between align-items-center mb-2';
        legendItem.innerHTML = `
            <div class="d-flex align-items-center">
                <div style="width: 12px; height: 12px; background-color: ${item.color}; border-radius: 50%; margin-right: 8px;"></div>
                <span>${item.status}</span>
            </div>
            <div>
                <strong>${item.count}</strong> (${percentage}%)
            </div>
        `;
        legendContainer.appendChild(legendItem);
    });
}

function updateRecentPatientsList(patients) {
    const container = document.getElementById('recentPatientsList');
    if (!container) return;

    container.innerHTML = '';

    if (!patients || patients.length === 0) {
        container.innerHTML = '<div class="text-center text-muted">No recent patients</div>';
        return;
    }

    patients.forEach(patient => {
        const patientItem = document.createElement('div');
        patientItem.className = 'patient-item';
        patientItem.innerHTML = `
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <div class="font-weight-bold">${patient.name}</div>
                    <div class="text-muted small">${patient.email}</div>
                    <div class="text-muted small">${patient.registeredAt}</div>
                </div>
                <span class="status-badge ${patient.status === 'Active' ? 'bg-success text-white' : 'bg-secondary text-white'}">
                    ${patient.status}
                </span>
            </div>
        `;
        container.appendChild(patientItem);
    });
}

// FIX: Improve changePeriod function
function changePeriod() {
    const period = document.getElementById('periodSelector').value;
    console.log('Changing period to:', period);
    console.log('Available data:', window.appointmentData);

    currentAppointmentPeriod = period;

    if (!window.appointmentData) {
        console.warn('No appointment data available');
        return;
    }

    let trendsData = [];
    let labels = [];

    switch (period) {
        case 'daily':
            if (window.appointmentData.dailyAppointmentTrends) {
                trendsData = window.appointmentData.dailyAppointmentTrends;
                labels = trendsData.map(t => t.day);
                console.log('Daily data:', trendsData);
            } else {
                console.warn('No daily appointment trends data');
            }
            break;
        case 'weekly':
            if (window.appointmentData.weeklyAppointmentTrends) {
                trendsData = window.appointmentData.weeklyAppointmentTrends;
                labels = trendsData.map(t => t.week);
                console.log('Weekly data:', trendsData);
            } else {
                console.warn('No weekly appointment trends data');
            }
            break;
        case 'monthly':
        default:
            if (window.appointmentData.appointmentTrends) {
                trendsData = window.appointmentData.appointmentTrends;
                labels = trendsData.map(t => t.month);
                console.log('Monthly data:', trendsData);
            } else {
                console.warn('No monthly appointment trends data');
            }
            break;
    }

    if (appointmentChart && trendsData.length > 0) {
        console.log('Updating chart with:', { labels, data: trendsData.map(t => t.count) });
        appointmentChart.data.labels = labels;
        appointmentChart.data.datasets[0].data = trendsData.map(t => t.count);
        appointmentChart.update();

        // Update period stats
        updatePeriodStats(trendsData.map(t => t.count));
    } else {
        console.warn('Cannot update chart - no data or chart not initialized');
    }
}

// Add confirmed appointments display
function updateConfirmedAppointments(data) {
    const confirmedElement = document.getElementById('confirmedAppointments');
    if (confirmedElement) {
        confirmedElement.textContent = data.confirmedAppointments || 0;
    }
}