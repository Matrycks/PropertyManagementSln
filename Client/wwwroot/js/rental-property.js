$(document).ready(function () {
  const $propertyForm = $("#propertyForm");
  const $unitForm = $("#unitForm");

  // PROPERTY MODAL
  $("#addPropertyBtn").on("click", function () {
    $propertyForm[0].reset();
    $propertyForm.attr("action", "/RentalProperty/Create");

    $("#propertyId").val("0");
    $("#propertyModalTitle").text("Add Property");

    $("#propertyActiveContainer").prop("hidden", true);
    $("#propertyIsActive").prop("checked", true);
  });

  $("#editPropertyBtn").on("click", async function () {
    const id = $(this).data("id");

    $propertyForm[0].reset();
    $propertyForm.attr("action", "/RentalProperty/Edit");

    try {
      const response = await $.ajax({
        url: `/RentalProperty/RentalPropertyDetailsForEdit?propertyId=${encodeURIComponent(id)}`,
        method: "GET",
        dataType: "json",
        headers: {
          Accept: "application/json",
        },
      });

      $.each(response, function (key, value) {
        const fieldName = key.charAt(0).toUpperCase() + key.slice(1);
        const $field = $propertyForm.find(`[name="${fieldName}"]`);

        if (!$field.length) return;

        if ($field.is(":checkbox")) {
          $field.prop("checked", Boolean(value));
        } else {
          $field.val(value ?? "");
        }
      });
    } catch (error) {
      alert(
        error.responseJSON?.message ||
          error.message ||
          "Unable to load the property.",
      );
    }

    const active = String($button.attr("data-active")).toLowerCase() === "true";
    $("#propertyIsActive").prop("checked", active);

    $("#propertyActiveContainer").prop("hidden", false);
  });

  // UNIT MODAL
  $("#addUnitBtn").on("click", async function () {
    const $button = $(this);

    $unitForm[0].reset();
    $unitForm.attr("action", "/RentalUnit/Create");

    $("#unitId").val("0");
    $("#unitPropertyId").val($button.data("property-id"));

    $("#unitPropertyName").text($button.data("property-name"));
    $("#unitModalTitle").text("Add Unit");

    $("#unitOccupiedContainer").prop("hidden", true);
    $("#unitIsOccupied").prop("checked", false);

    const floorPlans = await getFloorPlans();
    const $dropdown = $("#floorPlanId");

    $dropdown.empty();
    $dropdown.append($("<option>").val("").text("Select a floor plan"));

    floorPlans.forEach((plan) => {
      $dropdown.append($("<option>").val(plan.floorPlanId).text(plan.name));
    });
  });

  $("#editUnitBtn").on("click", async function () {
    const $button = $(this);
    const unitId = $button.data("id");

    $unitForm[0].reset();
    $unitForm.attr("action", "/RentalUnit/Edit");

    $("#unitModalTitle").text("Edit Unit");

    // Bind floor plans
    const floorPlans = await getFloorPlans();
    const $dropdown = $("#floorPlanId");

    $dropdown.empty();
    $dropdown.append($("<option>").val("").text("Select a floor plan"));

    floorPlans.forEach((plan) => {
      $dropdown.append($("<option>").val(plan.floorPlanId).text(plan.name));
    });

    // Bind unit data
    try {
      const response = await $.ajax({
        url: `/RentalUnit/FormData?unitId=${encodeURIComponent(unitId)}`,
        method: "GET",
        dataType: "json",
        headers: {
          Accept: "application/json",
        },
      });

      $.each(response, function (key, value) {
        const fieldName = key.charAt(0).toUpperCase() + key.slice(1);
        const $field = $unitForm.find(`[name="${fieldName}"]`);

        if (!$field.length) return;

        if ($field.is(":checkbox")) {
          $field.prop("checked", Boolean(value));
        } else {
          $field.val(value ?? "");
        }
      });
    } catch (error) {
      alert(
        error.responseJSON?.message ||
          error.message ||
          "Unable to load the unit.",
      );
    }

    $("#unitOccupiedContainer").prop("hidden", false);
  });

  async function getFloorPlans() {
    try {
      const response = await $.ajax({
        url: `/FloorPlan/ListOptions`,
        method: "GET",
        dataType: "json",
        headers: {
          Accept: "application/json",
        },
      });

      return response;
    } catch (error) {
      alert(
        error.responseJSON?.message ||
          error.message ||
          "Unable to retrieve floor plans.",
      );

      return [];
    }
  }
});
