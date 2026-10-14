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
            class: "btn btn-sm btn-primary btn-edit-residence",
            text: "Edit",
          })
            .attr("data-index", i)
            .attr("data-bs-toggle", "modal")
            .attr("data-bs-target", "#residenceModal"),
          $("<button>", {
            type: "button",
            class: "btn btn-sm btn-danger btn-delete-residence",
            text: "Delete",
          })
            .attr("data-index", i)
            .attr("data-bs-toggle", "modal")
            .attr("data-bs-target", "#residenceModal"),
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

  $("#addResidenceBtn").on("click", function () {
    $("#residenceModalTitle").text("Add Previous Residence");

    $("#ResidenceHistoryIndex").val("");
    $("#resAddress").val("");
    $("#resCity").val("");
    $("#resState").val("");
    $("#resZipCode").val("");
    $("#resMoveInDate").val("");
    $("#resMoveOutDate").val("");
    $("#resLandlordName").val("");
    $("#resLandlordPhone").val("");
  });

  $("#saveResidenceBtn").on("click", async function () {
    debugger;
    const form = document.getElementById("applicationForm");

    // Validate required fields
    if (!form.reportValidity()) {
      return;
    }

    const moveIn = $("#resMoveInDate").val();
    const moveOut = $("#resMoveOutDate").val();

    if (moveOut && moveOut < moveIn) {
      alert("Move-out date must be on or after move-in date.");
      return;
    }

    // Collect modal values
    const residence = {
      ResidenceHistoryId: Number($("#ResidenceHistoryId").val()) || 0,
      ApplicantId: Number($("#resApplicantId").val()) || 0,
      Address: $("#resAddress").val().trim(),
      City: $("#resCity").val().trim(),
      State: $("#resState").val().trim(),
      ZipCode: $("#resZipCode").val().trim(),
      MoveInDate: moveIn,
      MoveOutDate: moveOut || null,
      LandlordName: $("#resLandlordName").val().trim(),
      LandlordPhone: $("#resLandlordPhone").val().trim(),
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
    //form.reset();
    $("#ResidenceHistoryIndex").val("");
  });

  $(document).on("click", ".btn-edit-residence", async function () {
    const index = $(this).data("index");

    $("#residenceModalTitle").text("Edit Previous Residence");

    // Set modal fields
    var residence = residences[index];
    $("#ResidenceHistoryIndex").val(index);
    $("#resAddress").val(residence.Address);
    $("#resCity").val(residence.City);
    $("#resState").val(residence.State);
    $("#resZipCode").val(residence.ZipCode);
    $("#resMoveInDate").val(String(residence.MoveInDate).split("T")[0]);
    $("#resMoveOutDate").val(String(residence.MoveOutDate).split("T")[0]);
    $("#resLandlordName").val(residence.LandlordName);
    $("#resLandlordPhone").val(residence.LandlordPhone);
  });
});
