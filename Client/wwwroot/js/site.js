$(document).ready(function () {
  // Target all alerts, wait 4 seconds, fade out over 150ms, then remove from DOM
  $(".alert-dismissible")
    .delay(4000)
    .fadeOut(150, function () {
      $(this).remove();
    });
});

async function showModal(title, route) {
  $("#rentalPropertyModal").remove();

  // Build modal
  const modalHtml = `
    <div class="modal fade" id="rentalPropertyModal" tabindex="-1" aria-labelledby="rentalPropertyModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="rentalPropertyModalLabel">${title}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body" id="rentalPropertyModalBody">
            <div class="text-center py-4">
              <div class="spinner-border text-primary" role="status"></div>
              <p class="mt-2">Loading details...</p>
            </div>
          </div>
        </div>
      </div>
    </div>`;

  // Append the modal to the body
  $("body").append(modalHtml);

  const modalRawElement = $("#rentalPropertyModal")[0];
  const bsModalInstance = new bootstrap.Modal(modalRawElement);

  // Show the modal
  bsModalInstance.show();

  $("#rentalPropertyModal").on("hidden.bs.modal", function () {
    $(this).remove();
  });

  // Fetch view content asynchronously
  try {
    const response = await fetch(route);
    console.log("Response status:", response.status);

    if (!response.ok) {
      throw new Error("Failed to load property details.");
    }

    const htmlResult = await response.text();
    $("#rentalPropertyModalBody").html(htmlResult);
  } catch (error) {
    $("#rentalPropertyModalBody").text(
      "Unable to load rental property details.",
    );
    console.error(error);
  }
}

function showConfirmModal(event, message) {
  event.preventDefault();
  const form = event.target;

  $("#confirmModal").remove();
  $(".modal-backdrop").remove();

  const modalHtml = `
    <div class="modal fade" id="confirmModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title"><i class="bi bi-exclamation-triangle-fill me-2"></i> Confirm Delete</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body py-4">
                    <p class="mb-0 text-dark fs-5">${message}</p>
                </div>
                <div class="modal-footer bg-light">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" id="btnConfirm">Confirm</button>
                </div>
            </div>
        </div>
    </div>`;

  $("body").append(modalHtml);

  const modalRawElement = $("#confirmModal")[0];
  const bsModalInstance = new bootstrap.Modal(modalRawElement);

  bsModalInstance.show();

  // If the user clicks the explicit delete button, submit the raw HTML form
  $("#btnConfirm").on("click", function () {
    form.submit();
  });

  $modalRawElement.on("hidden.bs.modal", function () {
    $(this).remove();
    $(".modal-backdrop").remove();
  });
}
