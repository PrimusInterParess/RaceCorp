var count = 0;
var selectListId = count + 1;



function addAnotherItem(event) {
    event.preventDefault();

    var kvpArray;

    var fetch_status;
    fetch(`/api/difficulty/difficulties/`, {
        method: "GET",
        headers: {
            "Content-type": "application/json;charset=UTF-8"
        }
    })
        .then(function (response) {
            // Save the response status in a variable to use later.
            fetch_status = response.status;
            // Handle success
            // eg. Convert the response to JSON and return
            return response.json();
        })
        .then(function (json) {
            // Check if the response were success
            if (fetch_status == 200) {
                // Use the converted JSON

                kvpArray = json;

                var selectList = document.createElement("select");
                selectList.id = count;
                selectList.setAttribute('asp-for', `Traces[${count}].DifficultyId`);
                selectList.setAttribute('name', `Traces[${count}].DifficultyId`);
                selectList.classList.add('form-control');

                for (var i = 0; i < kvpArray.length; i++) {
                    var option = document.createElement("option");
                    option.value = kvpArray[i]['key'];
                    option.text = kvpArray[i]['value'];
                    selectList.appendChild(option);
                }

                var childElement = `
            <div class="form-control mb-3" id=block${count}>
               <div  class="input-group mb-3">
              <span class="input-group-text required" id="basic-addon1">Trace name</span>
               <input type="text" class="form-control" name='Traces[${count}].Name' placeholder="Trace Name">
               <span asp-validation-for='Traces[${count}].Name' class="text-danger"></span>
             </div>
               
            
             <div  class="input-group mb-3">
               <span class="input-group-text required" id="basic-addon1">Length</span>
               <input type="text" class="form-control" name='Traces[${count}].Length' placeholder="Trace Length in kilometers">
               <span asp-validation-for='Traces[${count}].Length' class="text-danger"></span>
             </div>
             <div id="select${count}" class="input-group mb-3">
               <span class="input-group-text required" id="basic-addon3">Difficulty</span>
               <span asp-validation-for='Traces[${count}].DifficultyId' class="text-danger"></span>
             </div>
             <div class="input-group mb-3">
               <span class="input-group-text required" id="basic-addon2">Control time</span>
               <input type="text" class="form-control" name='Traces[${count}].ControlTime'  placeholder="Trace Control time in hours :1 , 2 , 3">
               <span asp-validation-for='Traces[${count}].ControlTime' class="text-danger"></span>
             </div>
             <div class="input-group mb-3">
               <span class="input-group-text required" id="basic-addon3">Start Time</span>
                       <input type="datetime-local" class="form-control" name='Traces[${count}].StartTime' placeholder="">
               <span asp-validation-for='Traces[${count}].StartTime' class="text-danger"></span>
             </div>
             <div class="input-group mb-3">
                <span class="input-group-text required">Gpx</span>
                <input  type="file" name='Traces[${count}].GpxFile' class="form-control" id='Traces[${count}].GpxFile' >
             </div>
             </div>`;

                $("#ol").append(childElement);

                var divElement = document.getElementById(`select${count}`);
                divElement.appendChild(selectList);

                var createDeleteChild = document.createElement('button');
                createDeleteChild.setAttribute('href', '#');
                createDeleteChild.textContent = 'Delete';
                createDeleteChild.addEventListener('click', deleteItem);
                createDeleteChild.classList.add('btn');
                createDeleteChild.classList.add('border');
                createDeleteChild.classList.add('border-dark');
                createDeleteChild.classList.add('text-danger');

                var divEl = document.getElementById(`block${count}`);
                var btnsDivEl = document.getElementById("btns");

                if (btnsDivEl.childElementCount == 1) {
                    btnsDivEl.appendChild(createDeleteChild);
                }
                count += 1;

                var olDivEl = document.getElementById("ol");

                function deleteItem(event) {
                    event.preventDefault();
                    document.getElementById(`block${count - 1}`).remove();

                    if (olDivEl.childElementCount == 0) {
                        createDeleteChild.remove();
                    }
                    count -= 1;
                }
            }

        })
        .catch(function (error) {
            // Catch errors
            console.log(error);
        });

}

function getElements(evnt) {

    var toAttach = `
                                    <label class="mb-2 mt-2"><b>Traces</b></label>
                                      <div id="btns">
                                          <button class="btn border border-dark" onclick="addAnotherItem(event)">add</button>
                                      </div>
                                      <div class="mb-2 mt-2" id="ol">
                                      </div>`;

    $("#attachHere").append(toAttach);
}