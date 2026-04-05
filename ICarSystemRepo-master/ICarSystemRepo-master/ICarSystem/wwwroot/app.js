// ===== ICarSystem Web Application =====

// ===== Data Store (Mock Data mirroring C# Program.cs) =====
const dataStore = {
  currentUser: null,
  userType: null,
  vehicles: [
    {
      vehicleID: 1,
      make: "Toyota",
      model: "Camry",
      year: 2020,
      mileage: 15000,
      photos: "url-to-photo",
      insuranceNo: "INS123",
      insuranceCoverage: "Full Coverage",
      rentalRate: 100.0,
      availabilities: [
        { id: 1, startDate: "2024-08-01", endDate: "2024-08-10" },
        { id: 2, startDate: "2024-08-15", endDate: "2024-08-20" },
      ],
      bookings: [],
    },
    {
      vehicleID: 2,
      make: "Honda",
      model: "Civic",
      year: 2019,
      mileage: 20000,
      photos: "url-to-photo",
      insuranceNo: "INS456",
      insuranceCoverage: "Full Coverage",
      rentalRate: 90.0,
      availabilities: [],
      bookings: [],
    },
    {
      vehicleID: 3,
      make: "Toyota",
      model: "Camry",
      year: 2021,
      mileage: 5000,
      photos: "photos.jpg",
      insuranceNo: "INS123",
      insuranceCoverage: "Full Coverage",
      rentalRate: 70.0,
      availabilities: [
        { id: 1, startDate: "2024-10-01", endDate: "2024-10-10" },
        { id: 2, startDate: "2024-10-15", endDate: "2024-10-20" },
        { id: 3, startDate: "2024-11-01", endDate: "2024-11-05" },
        { id: 4, startDate: "2024-11-10", endDate: "2024-11-15" },
      ],
      bookings: [],
    },
    {
      vehicleID: 4,
      make: "Honda",
      model: "Civic",
      year: 2020,
      mileage: 3000,
      photos: "photos2.jpg",
      insuranceNo: "INS456",
      insuranceCoverage: "Full Coverage",
      rentalRate: 90.0,
      availabilities: [
        { id: 1, startDate: "2024-09-01", endDate: "2024-09-05" },
        { id: 2, startDate: "2024-09-10", endDate: "2024-09-15" },
        { id: 3, startDate: "2024-10-01", endDate: "2024-10-05" },
        { id: 4, startDate: "2024-10-10", endDate: "2024-10-15" },
      ],
      bookings: [],
    },
  ],
  bookings: [
    {
      bookingID: "BKG001",
      vehicleId: 1,
      startDate: "2024-08-05T09:00:00",
      endDate: "2024-08-10T09:00:00",
      totalPrice: 500.0,
      pickupOption: "station",
      returnOption: "station",
      pickupStation: { stationID: "1", location: "Changi Airport Terminal 3" },
      returnStation: { stationID: "2", location: "Marina Bay Sands" },
      deliveryFee: 0,
      returnFee: 0,
    },
  ],
  stations: [
    { stationID: "1", location: "Changi Airport Terminal 3" },
    { stationID: "2", location: "Marina Bay Sands" },
    { stationID: "3", location: "Jurong East MRT" },
  ],
  users: {
    renter: {
      username: "Aiman",
      password: "password123",
      renterId: 1,
      isVerified: true,
    },
    carowner: { username: "Jovan", password: "securePass" },
  },
};

// ===== Navigation & Page Management =====
function showPage(page) {
  // Hide all pages
  document
    .querySelectorAll(".page")
    .forEach((p) => p.classList.remove("active"));

  // Update nav links
  document
    .querySelectorAll(".nav-link")
    .forEach((link) => link.classList.remove("active"));

  const userInfo = document.getElementById("userInfo");
  const mainNav = document.getElementById("mainNav");

  switch (page) {
    case "home":
      document.getElementById("homePage").classList.add("active");
      document.querySelector('[data-page="home"]')?.classList.add("active");
      userInfo.style.display = "none";
      mainNav.style.display = "flex";
      break;
    case "renter":
      if (dataStore.currentUser && dataStore.userType === "renter") {
        document.getElementById("renterDashboard").classList.add("active");
        renderVehicles();
        renderBookings();
      } else {
        document.getElementById("renterLoginPage").classList.add("active");
      }
      userInfo.style.display = dataStore.currentUser ? "flex" : "none";
      mainNav.style.display = "flex";
      break;
    case "carowner":
      if (dataStore.currentUser && dataStore.userType === "carowner") {
        document.getElementById("carownerDashboard").classList.add("active");
        renderOwnerVehicles();
        populateVehicleDropdown();
        renderSchedules();
      } else {
        document.getElementById("carownerLoginPage").classList.add("active");
      }
      userInfo.style.display = dataStore.currentUser ? "flex" : "none";
      mainNav.style.display = "flex";
      break;
  }
}

// ===== Authentication =====
document
  .getElementById("renterLoginForm")
  .addEventListener("submit", function (e) {
    e.preventDefault();
    const username = document.getElementById("renterUsername").value;
    const password = document.getElementById("renterPassword").value;

    if (
      username === dataStore.users.renter.username &&
      password === dataStore.users.renter.password
    ) {
      dataStore.currentUser = dataStore.users.renter;
      dataStore.userType = "renter";
      document.getElementById("usernameDisplay").textContent =
        `Welcome, ${username}`;
      showToast("Login successful!", "success");
      showPage("renter");
    } else {
      showToast("Invalid credentials. Please try again.", "error");
    }
  });

document
  .getElementById("carownerLoginForm")
  .addEventListener("submit", function (e) {
    e.preventDefault();
    const username = document.getElementById("carownerUsername").value;
    const password = document.getElementById("carownerPassword").value;

    if (
      username === dataStore.users.carowner.username &&
      password === dataStore.users.carowner.password
    ) {
      dataStore.currentUser = dataStore.users.carowner;
      dataStore.userType = "carowner";
      document.getElementById("usernameDisplay").textContent =
        `Welcome, ${username}`;
      showToast("Login successful!", "success");
      showPage("carowner");
    } else {
      showToast("Invalid credentials. Please try again.", "error");
    }
  });

document.getElementById("logoutBtn").addEventListener("click", function () {
  dataStore.currentUser = null;
  dataStore.userType = null;
  showToast("Logged out successfully", "success");
  showPage("home");
});

// ===== Tab Navigation =====
document.querySelectorAll(".tab-btn").forEach((btn) => {
  btn.addEventListener("click", function () {
    const tabId = this.dataset.tab;
    const parent = this.closest(".page");

    // Update tab buttons
    parent
      .querySelectorAll(".tab-btn")
      .forEach((b) => b.classList.remove("active"));
    this.classList.add("active");

    // Update tab content
    parent
      .querySelectorAll(".tab-content")
      .forEach((content) => content.classList.remove("active"));
    parent
      .querySelector(`#${tabId.replace("-", "")}Tab`)
      ?.classList.add("active") ||
      parent.querySelector(`#${tabId}Tab`)?.classList.add("active");
  });
});

// ===== Render Vehicles (Renter View) =====
function renderVehicles() {
  const container = document.getElementById("vehiclesList");

  if (dataStore.vehicles.length === 0) {
    container.innerHTML = `
            <div class="empty-state">
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M14 16H9m10 0h3v-3.15a1 1 0 0 0-.84-.99L16 11l-2.7-3.6a1 1 0 0 0-.8-.4H5.24a1 1 0 0 0-.8.4L2 11v5h3m3-6h.01M17 10h.01M6 16a2 2 0 1 0 4 0a2 2 0 1 0-4 0m8 0a2 2 0 1 0 4 0a2 2 0 1 0-4 0"/>
                </svg>
                <h3>No vehicles available</h3>
                <p>Check back later for available vehicles</p>
            </div>
        `;
    return;
  }

  container.innerHTML = dataStore.vehicles
    .map(
      (vehicle) => `
        <div class="vehicle-card">
            <div class="vehicle-image">
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
                    <path d="M14 16H9m10 0h3v-3.15a1 1 0 0 0-.84-.99L16 11l-2.7-3.6a1 1 0 0 0-.8-.4H5.24a1 1 0 0 0-.8.4L2 11v5h3m3-6h.01M17 10h.01M6 16a2 2 0 1 0 4 0a2 2 0 1 0-4 0m8 0a2 2 0 1 0 4 0a2 2 0 1 0-4 0"/>
                </svg>
            </div>
            <div class="vehicle-details">
                <h3>${vehicle.make}</h3>
                <p class="vehicle-model">${vehicle.model}</p>
                <div class="vehicle-info">
                    <div class="vehicle-info-item"><strong>Year:</strong> ${vehicle.year}</div>
                    <div class="vehicle-info-item"><strong>Mileage:</strong> ${vehicle.mileage.toLocaleString()} km</div>
                    <div class="vehicle-info-item"><strong>Insurance:</strong> ${vehicle.insuranceCoverage}</div>
                    <div class="vehicle-info-item"><strong>Availability:</strong> ${vehicle.availabilities.length > 0 ? "Available" : "Not Available"}</div>
                </div>
                <div class="vehicle-price">S$${vehicle.rentalRate.toFixed(2)}/day</div>
                <div class="vehicle-actions">
                    <button class="btn btn-primary" onclick="openRentModal(${vehicle.vehicleID})" ${vehicle.availabilities.length === 0 ? "disabled" : ""}>
                        ${vehicle.availabilities.length > 0 ? "Rent Now" : "Not Available"}
                    </button>
                </div>
            </div>
        </div>
    `,
    )
    .join("");
}

// ===== Render Bookings (Renter View) =====
function renderBookings() {
  const container = document.getElementById("bookingsList");
  const renterBookings = dataStore.bookings;

  if (renterBookings.length === 0) {
    container.innerHTML = `
            <div class="empty-state">
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <rect x="3" y="4" width="18" height="18" rx="2" ry="2"/>
                    <line x1="16" y1="2" x2="16" y2="6"/>
                    <line x1="8" y1="2" x2="8" y2="6"/>
                    <line x1="3" y1="10" x2="21" y2="10"/>
                </svg>
                <h3>No bookings yet</h3>
                <p>Browse available vehicles to make your first booking</p>
            </div>
        `;
    return;
  }

  container.innerHTML = renterBookings
    .map((booking) => {
      const vehicle = dataStore.vehicles.find(
        (v) => v.vehicleID === booking.vehicleId,
      );
      return `
            <div class="booking-card">
                <div class="booking-header">
                    <div>
                        <h3>${vehicle ? vehicle.make + " " + vehicle.model : "Unknown Vehicle"}</h3>
                        <span class="booking-id">Booking ID: ${booking.bookingID}</span>
                    </div>
                    <span class="booking-status confirmed">Confirmed</span>
                </div>
                <div class="booking-dates">
                    <div class="booking-date-item">
                        <label>Start Date</label>
                        <span>${new Date(booking.startDate).toLocaleDateString("en-SG", { day: "2-digit", month: "short", year: "numeric" })}</span>
                    </div>
                    <div class="booking-date-item">
                        <label>End Date</label>
                        <span>${new Date(booking.endDate).toLocaleDateString("en-SG", { day: "2-digit", month: "short", year: "numeric" })}</span>
                    </div>
                </div>
                <div class="booking-footer">
                    <div class="booking-price">S$${booking.totalPrice.toFixed(2)}</div>
                    <button class="btn btn-outline" onclick="viewBookingDetails('${booking.bookingID}')">View Details</button>
                </div>
            </div>
        `;
    })
    .join("");
}

// ===== Render Owner Vehicles =====
function renderOwnerVehicles() {
  const container = document.getElementById("ownerVehiclesList");
  const ownerVehicles = dataStore.vehicles;

  if (ownerVehicles.length === 0) {
    container.innerHTML = `
            <div class="empty-state">
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M14 16H9m10 0h3v-3.15a1 1 0 0 0-.84-.99L16 11l-2.7-3.6a1 1 0 0 0-.8-.4H5.24a1 1 0 0 0-.8.4L2 11v5h3m3-6h.01M17 10h.01M6 16a2 2 0 1 0 4 0a2 2 0 1 0-4 0m8 0a2 2 0 1 0 4 0a2 2 0 1 0-4 0"/>
                </svg>
                <h3>No vehicles registered</h3>
                <p>Register your first vehicle to start earning</p>
            </div>
        `;
    return;
  }

  container.innerHTML = ownerVehicles
    .map(
      (vehicle) => `
        <div class="vehicle-card">
            <div class="vehicle-image">
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
                    <path d="M14 16H9m10 0h3v-3.15a1 1 0 0 0-.84-.99L16 11l-2.7-3.6a1 1 0 0 0-.8-.4H5.24a1 1 0 0 0-.8.4L2 11v5h3m3-6h.01M17 10h.01M6 16a2 2 0 1 0 4 0a2 2 0 1 0-4 0m8 0a2 2 0 1 0 4 0a2 2 0 1 0-4 0"/>
                </svg>
            </div>
            <div class="vehicle-details">
                <h3>${vehicle.make}</h3>
                <p class="vehicle-model">${vehicle.model}</p>
                <div class="vehicle-info">
                    <div class="vehicle-info-item"><strong>Year:</strong> ${vehicle.year}</div>
                    <div class="vehicle-info-item"><strong>Mileage:</strong> ${vehicle.mileage.toLocaleString()} km</div>
                    <div class="vehicle-info-item"><strong>Insurance:</strong> ${vehicle.insuranceNo}</div>
                    <div class="vehicle-info-item"><strong>Coverage:</strong> ${vehicle.insuranceCoverage}</div>
                </div>
                <div class="vehicle-price">S$${vehicle.rentalRate.toFixed(2)}/day</div>
                <div class="vehicle-actions">
                    <button class="btn btn-outline" onclick="viewVehicleAvailability(${vehicle.vehicleID})">View Availability</button>
                </div>
            </div>
        </div>
    `,
    )
    .join("");
}

// ===== Populate Vehicle Dropdown for Scheduling =====
function populateVehicleDropdown() {
  const select = document.getElementById("scheduleVehicle");
  select.innerHTML = '<option value="">Choose a vehicle...</option>';

  dataStore.vehicles.forEach((vehicle) => {
    select.innerHTML += `<option value="${vehicle.vehicleID}">${vehicle.make} ${vehicle.model} (ID: ${vehicle.vehicleID})</option>`;
  });
}

// ===== Render Schedules =====
function renderSchedules() {
  const container = document.getElementById("schedulesList");
  const vehicleId = parseInt(document.getElementById("scheduleVehicle").value);

  if (!vehicleId) {
    container.innerHTML =
      '<p class="text-muted">Select a vehicle to view its schedules</p>';
    return;
  }

  const vehicle = dataStore.vehicles.find((v) => v.vehicleID === vehicleId);
  if (!vehicle || vehicle.availabilities.length === 0) {
    container.innerHTML =
      '<p class="text-muted">No schedules found for this vehicle</p>';
    return;
  }

  container.innerHTML = vehicle.availabilities
    .map(
      (avail) => `
        <div class="schedule-item">
            <div>
                <span class="schedule-id">ID: ${avail.id}</span>
                <div class="schedule-dates">${formatDate(avail.startDate)} - ${formatDate(avail.endDate)}</div>
            </div>
            <button class="btn-delete" onclick="deleteAvailability(${vehicleId}, ${avail.id})">Delete</button>
        </div>
    `,
    )
    .join("");
}

document
  .getElementById("scheduleVehicle")
  .addEventListener("change", renderSchedules);

// ===== Schedule Form =====
document
  .getElementById("scheduleForm")
  .addEventListener("submit", function (e) {
    e.preventDefault();

    const vehicleId = parseInt(
      document.getElementById("scheduleVehicle").value,
    );
    const startDate = document.getElementById("startDate").value;
    const endDate = document.getElementById("endDate").value;

    if (!vehicleId) {
      showToast("Please select a vehicle", "error");
      return;
    }

    if (!startDate || !endDate) {
      showToast("Please enter both start and end dates", "error");
      return;
    }

    if (new Date(endDate) <= new Date(startDate)) {
      showToast("End date must be after start date", "error");
      return;
    }

    const vehicle = dataStore.vehicles.find((v) => v.vehicleID === vehicleId);
    if (!vehicle) {
      showToast("Vehicle not found", "error");
      return;
    }

    // Check for overlaps
    const hasOverlap = vehicle.availabilities.some(
      (avail) =>
        new Date(startDate) < new Date(avail.endDate) &&
        new Date(endDate) > new Date(avail.startDate),
    );

    if (hasOverlap) {
      showToast("The entered dates overlap with an existing schedule", "error");
      return;
    }

    // Add new availability
    const newId =
      vehicle.availabilities.length > 0
        ? Math.max(...vehicle.availabilities.map((a) => a.id)) + 1
        : 1;
    vehicle.availabilities.push({
      id: newId,
      startDate: startDate,
      endDate: endDate,
    });

    showToast("Availability scheduled successfully!", "success");
    renderSchedules();

    // Reset form
    document.getElementById("startDate").value = "";
    document.getElementById("endDate").value = "";
  });

// ===== Delete Availability =====
function deleteAvailability(vehicleId, availabilityId) {
  if (!confirm("Are you sure you want to delete this availability?")) return;

  const vehicle = dataStore.vehicles.find((v) => v.vehicleID === vehicleId);
  if (vehicle) {
    vehicle.availabilities = vehicle.availabilities.filter(
      (a) => a.id !== availabilityId,
    );
    showToast("Availability deleted successfully", "success");
    renderSchedules();
  }
}

// ===== Rent Car Modal =====
function openRentModal(vehicleId) {
  const vehicle = dataStore.vehicles.find((v) => v.vehicleID === vehicleId);
  if (!vehicle) return;

  document.getElementById("rentVehicleId").value = vehicleId;
  document.getElementById("rentCarVehicleInfo").innerHTML = `
        <div class="vehicle-card" style="margin-bottom: 20px;">
            <div class="vehicle-details">
                <h3>${vehicle.make} ${vehicle.model}</h3>
                <p class="vehicle-model">Year: ${vehicle.year} | Mileage: ${vehicle.mileage.toLocaleString()} km</p>
                <div class="vehicle-price">S$${vehicle.rentalRate.toFixed(2)}/day</div>
            </div>
        </div>
    `;

  // Set min date to today
  const today = new Date().toISOString().split("T")[0];
  document.getElementById("rentStartDate").min = today;
  document.getElementById("rentEndDate").min = today;

  // Reset form
  document.getElementById("rentCarForm").reset();
  document.getElementById("bookingSummary").style.display = "none";
  document.getElementById("confirmBookingBtn").style.display = "none";
  document.getElementById("calculatePriceBtn").style.display = "inline-flex";

  // Show pickup station group by default
  document.getElementById("pickupStationGroup").style.display = "block";
  document.getElementById("pickupDeliveryGroup").style.display = "none";
  document.getElementById("returnStationGroup").style.display = "block";
  document.getElementById("returnDeliveryGroup").style.display = "none";

  openModal("rentCarModal");
}

// ===== Pickup/Return Option Toggle =====
document.querySelectorAll('input[name="pickupOption"]').forEach((radio) => {
  radio.addEventListener("change", function () {
    if (this.value === "station") {
      document.getElementById("pickupStationGroup").style.display = "block";
      document.getElementById("pickupDeliveryGroup").style.display = "none";
    } else {
      document.getElementById("pickupStationGroup").style.display = "none";
      document.getElementById("pickupDeliveryGroup").style.display = "block";
    }
  });
});

document.querySelectorAll('input[name="returnOption"]').forEach((radio) => {
  radio.addEventListener("change", function () {
    if (this.value === "station") {
      document.getElementById("returnStationGroup").style.display = "block";
      document.getElementById("returnDeliveryGroup").style.display = "none";
    } else {
      document.getElementById("returnStationGroup").style.display = "none";
      document.getElementById("returnDeliveryGroup").style.display = "block";
    }
  });
});

// ===== Calculate Booking Price =====
function calculateBookingPrice() {
  const vehicleId = parseInt(document.getElementById("rentVehicleId").value);
  const startDate = document.getElementById("rentStartDate").value;
  const endDate = document.getElementById("rentEndDate").value;

  if (!startDate || !endDate) {
    showToast("Please select start and end dates", "error");
    return;
  }

  if (new Date(endDate) <= new Date(startDate)) {
    showToast("End date must be after start date", "error");
    return;
  }

  const vehicle = dataStore.vehicles.find((v) => v.vehicleID === vehicleId);
  if (!vehicle) return;

  const days = Math.ceil(
    (new Date(endDate) - new Date(startDate)) / (1000 * 60 * 60 * 24),
  );
  const rentalCost = days * vehicle.rentalRate;

  const pickupOption = document.querySelector(
    'input[name="pickupOption"]:checked',
  ).value;
  const returnOption = document.querySelector(
    'input[name="returnOption"]:checked',
  ).value;

  let deliveryFee = 0;
  let returnFee = 0;

  if (pickupOption === "delivery") {
    deliveryFee = 25.0; // Flat delivery fee
  }
  if (returnOption === "delivery") {
    returnFee = 25.0; // Flat return fee
  }

  const totalPrice = rentalCost + deliveryFee + returnFee;

  // Show summary
  const summaryContent = document.getElementById("summaryContent");
  summaryContent.innerHTML = `
        <div class="summary-row">
            <span>Rental (${days} days x S$${vehicle.rentalRate.toFixed(2)})</span>
            <span>S$${rentalCost.toFixed(2)}</span>
        </div>
        <div class="summary-row">
            <span>Pickup ${pickupOption === "delivery" ? "Delivery Fee" : "(Station)"}</span>
            <span>${pickupOption === "delivery" ? "S$" + deliveryFee.toFixed(2) : "Free"}</span>
        </div>
        <div class="summary-row">
            <span>Return ${returnOption === "delivery" ? "Delivery Fee" : "(Station)"}</span>
            <span>${returnOption === "delivery" ? "S$" + returnFee.toFixed(2) : "Free"}</span>
        </div>
        <div class="summary-row">
            <span>Total</span>
            <span>S$${totalPrice.toFixed(2)}</span>
        </div>
    `;

  document.getElementById("bookingSummary").style.display = "block";
  document.getElementById("confirmBookingBtn").style.display = "inline-flex";
  document.getElementById("calculatePriceBtn").style.display = "none";
}

// ===== Confirm Booking =====
document.getElementById("rentCarForm").addEventListener("submit", function (e) {
  e.preventDefault();

  const vehicleId = parseInt(document.getElementById("rentVehicleId").value);
  const startDate = document.getElementById("rentStartDate").value;
  const endDate = document.getElementById("rentEndDate").value;
  const pickupOption = document.querySelector(
    'input[name="pickupOption"]:checked',
  ).value;
  const returnOption = document.querySelector(
    'input[name="returnOption"]:checked',
  ).value;

  const vehicle = dataStore.vehicles.find((v) => v.vehicleID === vehicleId);
  if (!vehicle) return;

  const days = Math.ceil(
    (new Date(endDate) - new Date(startDate)) / (1000 * 60 * 60 * 24),
  );
  const rentalCost = days * vehicle.rentalRate;

  let deliveryFee = pickupOption === "delivery" ? 25.0 : 0;
  let returnFee = returnOption === "delivery" ? 25.0 : 0;
  const totalPrice = rentalCost + deliveryFee + returnFee;

  // Create booking
  const newBookingId =
    "BKG" + String(dataStore.bookings.length + 1).padStart(3, "0");
  const newBooking = {
    bookingID: newBookingId,
    vehicleId: vehicleId,
    startDate: startDate + "T09:00:00",
    endDate: endDate + "T09:00:00",
    totalPrice: totalPrice,
    pickupOption: pickupOption,
    returnOption: returnOption,
    pickupStation:
      pickupOption === "station"
        ? dataStore.stations.find(
            (s) =>
              s.stationID === document.getElementById("pickupStation").value,
          )
        : null,
    returnStation:
      returnOption === "station"
        ? dataStore.stations.find(
            (s) =>
              s.stationID === document.getElementById("returnStation").value,
          )
        : null,
    pickupDeliveryDetails:
      pickupOption === "delivery"
        ? {
            name: document.getElementById("pickupName").value,
            contactNo: document.getElementById("pickupContact").value,
            address: document.getElementById("pickupAddress").value,
          }
        : null,
    returnDeliveryDetails:
      returnOption === "delivery"
        ? {
            name: document.getElementById("returnName").value,
            contactNo: document.getElementById("returnContact").value,
            address: document.getElementById("returnAddress").value,
          }
        : null,
    deliveryFee: deliveryFee,
    returnFee: returnFee,
  };

  dataStore.bookings.push(newBooking);

  showToast(`Booking ${newBookingId} confirmed successfully!`, "success");
  closeModal("rentCarModal");
  renderBookings();
});

// ===== View Booking Details =====
function viewBookingDetails(bookingId) {
  const booking = dataStore.bookings.find((b) => b.bookingID === bookingId);
  if (!booking) return;

  const vehicle = dataStore.vehicles.find(
    (v) => v.vehicleID === booking.vehicleId,
  );

  const content = document.getElementById("bookingDetailsContent");
  content.innerHTML = `
        <div class="booking-summary" style="display: block;">
            <h4>Booking ${booking.bookingID}</h4>
            <div class="summary-row">
                <span>Vehicle</span>
                <span>${vehicle ? vehicle.make + " " + vehicle.model : "Unknown"}</span>
            </div>
            <div class="summary-row">
                <span>Start Date</span>
                <span>${new Date(booking.startDate).toLocaleDateString("en-SG", { day: "2-digit", month: "long", year: "numeric" })}</span>
            </div>
            <div class="summary-row">
                <span>End Date</span>
                <span>${new Date(booking.endDate).toLocaleDateString("en-SG", { day: "2-digit", month: "long", year: "numeric" })}</span>
            </div>
            <div class="summary-row">
                <span>Pickup Option</span>
                <span>${booking.pickupOption === "station" ? "Station: " + (booking.pickupStation?.location || "N/A") : "Delivery"}</span>
            </div>
            <div class="summary-row">
                <span>Return Option</span>
                <span>${booking.returnOption === "station" ? "Station: " + (booking.returnStation?.location || "N/A") : "Delivery"}</span>
            </div>
            ${
              booking.pickupDeliveryDetails
                ? `
            <div class="summary-row">
                <span>Pickup Delivery Details</span>
                <span>${booking.pickupDeliveryDetails.name}, ${booking.pickupDeliveryDetails.contactNo}, ${booking.pickupDeliveryDetails.address}</span>
            </div>
            `
                : ""
            }
            ${
              booking.returnDeliveryDetails
                ? `
            <div class="summary-row">
                <span>Return Delivery Details</span>
                <span>${booking.returnDeliveryDetails.name}, ${booking.returnDeliveryDetails.contactNo}, ${booking.returnDeliveryDetails.address}</span>
            </div>
            `
                : ""
            }
            <div class="summary-row">
                <span>Delivery Fee</span>
                <span>S$${booking.deliveryFee.toFixed(2)}</span>
            </div>
            <div class="summary-row">
                <span>Return Fee</span>
                <span>S$${booking.returnFee.toFixed(2)}</span>
            </div>
            <div class="summary-row">
                <span>Total Amount</span>
                <span>S$${booking.totalPrice.toFixed(2)}</span>
            </div>
        </div>
    `;

  openModal("bookingDetailsModal");
}

// ===== View Vehicle Availability =====
function viewVehicleAvailability(vehicleId) {
  const vehicle = dataStore.vehicles.find((v) => v.vehicleID === vehicleId);
  if (!vehicle) return;

  // Switch to schedule tab and select the vehicle
  document.querySelector('[data-tab="schedule-availability"]')?.click();
  document.getElementById("scheduleVehicle").value = vehicleId;
  renderSchedules();
}

// ===== Modal Functions =====
function openModal(modalId) {
  document.getElementById(modalId).classList.add("active");
  document.body.style.overflow = "hidden";
}

function closeModal(modalId) {
  document.getElementById(modalId).classList.remove("active");
  document.body.style.overflow = "";
}

// Close modal on backdrop click
document.querySelectorAll(".modal").forEach((modal) => {
  modal.addEventListener("click", function (e) {
    if (e.target === this) {
      this.classList.remove("active");
      document.body.style.overflow = "";
    }
  });
});

// ===== Toast Notification =====
function showToast(message, type = "info") {
  const toast = document.getElementById("toast");
  toast.textContent = message;
  toast.className = "toast " + type;

  setTimeout(() => toast.classList.add("show"), 10);
  setTimeout(() => {
    toast.classList.remove("show");
  }, 3000);
}

// ===== Utility Functions =====
function formatDate(dateString) {
  const date = new Date(dateString);
  return date.toLocaleDateString("en-SG", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  });
}

// ===== Initialize =====
document.addEventListener("DOMContentLoaded", function () {
  // Set up nav links
  document.querySelectorAll(".nav-link").forEach((link) => {
    link.addEventListener("click", function (e) {
      e.preventDefault();
      showPage(this.dataset.page);
    });
  });

  // Show home page by default
  showPage("home");
});
