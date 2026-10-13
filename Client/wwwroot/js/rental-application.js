$(document).ready(function () {
  let residences = [];

  function addResidence(residence) {
    residences.push(residence);
    renderResidences();
  }

  function updateResidence(index, residence) {
    residences[index] = residence;
    renderResidences();
  }

  function deleteResidence(index) {
    residences.splice(index, 1);
    renderResidences();
  }

  function renderResidences() {
    const $list = $("#residenceList");
    $list.empty();

    residences.forEach((r, i) => {
      const $card = $("<div>", {
        class: "card mb-3 residence-card",
      });

      $("<div>", {
        class: "card-body",
      })
        .append(
          $("<strong>").text(r.Address),
          $("<div>").text(`${r.City}, ${r.State} ${r.ZipCode}`),
          $("<button>", {
            type: "button",
            class: "btn btn-sm btn-primary edit-residence",
            text: "Edit",
          }).attr("data-index", i),
          $("<button>", {
            type: "button",
            class: "btn btn-sm btn-danger delete-residence",
            text: "Delete",
          }).attr("data-index", i),
        )
        .appendTo($card);

      Object.entries(r).forEach(([key, value]) => {
        $("<input>", {
          type: "hidden",
          name: `ResidenceHistory[${i}].${key}`,
          value: value ?? "",
        }).appendTo($card);
      });

      $list.append($card);
    });
  }

  $("#saveResidenceBtn").on("click", async function () {
    const form = document.getElementById("residenceForm");

    // Validate required fields
    if (!form.reportValidity()) {
      return;
    }

    const moveIn = $("#MoveInDate").val();
    const moveOut = $("#MoveOutDate").val();

    if (moveOut && moveOut < moveIn) {
      alert("Move-out date must be on or after move-in date.");
      return;
    }

    // Collect modal values
    const residence = {
      ResidenceHistoryId: Number($("#ResidenceHistoryId").val()) || 0,
      ApplicantId: Number($("#ApplicantId").val()) || 0,
      Address: $("#Address").val().trim(),
      City: $("#City").val().trim(),
      State: $("#State").val().trim(),
      ZipCode: $("#ZipCode").val().trim(),
      MoveInDate: moveIn,
      MoveOutDate: moveOut || null,
      LandlordName: $("#LandlordName").val().trim(),
      LandlordPhone: $("#LandlordPhone").val().trim(),
    };

    const index = $("#ResidenceHistoryIndex").val();

    if (index !== "" && index !== undefined) {
      // Update an existing residence
      updateResidence(Number(index), residence);
    } else {
      // Add a new residence
      addResidence(residence);
    }

    // Close the modal
    bootstrap.Modal.getOrCreateInstance(
      document.getElementById("residenceModal"),
    ).hide();

    // Reset modal fields
    form.reset();
    $("#ResidenceHistoryIndex").val("");
  });
});
