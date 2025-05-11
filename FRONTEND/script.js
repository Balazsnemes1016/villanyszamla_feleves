document.getElementById("calculateBtn").addEventListener("click", async () => { //ez figyeli magat a gombot 
    const price = parseFloat(document.getElementById("price").value);
    const matrixText = document.getElementById("matrix").value;
  
    if (isNaN(price) || !matrixText.trim()) {
      alert("Adj meg érvényes egységárat és fogyasztási adatokat!");
      return; // ellenorzes hogy a megadott adat ervenyes 
    }
  
    const payload = { //Backendnek ertekatadas
      unitPrice: price,
      matrix: matrixText
    };
  
    try {
        const response = await fetch("https://localhost:7208/api/villanyszamla/szamitas", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
          });
          
  
      if (!response.ok) {
        throw new Error("Hibás válasz a szervertől");
      }
  
      const data = await response.json();
      renderTable(data);
  
    } catch (error) {
      alert("Hiba történt: " + error.message);
    }
  });
  
  function renderTable(data) {
    const { haviDijak, evesDijak, kedvezmenyesEvek } = data;
    const honapok = [
      "Január", "Február", "Március", "Április", "Május", "Június",
      "Július", "Augusztus", "Szeptember", "Október", "November", "December"
    ];
  
    const evek = Object.keys(haviDijak);
    const table = document.getElementById("resultTable");
    table.innerHTML = "";
  
    
    // Fejléc
    const headerRow = document.createElement("tr");
    headerRow.innerHTML = `<th>Hónap</th>` + evek.map(ev => {
      const cls = kedvezmenyesEvek.includes(parseInt(ev)) ? "table-success-col" : "";
      return `<th class="${cls}">${ev}</th>`;
    }).join("");
    table.appendChild(headerRow);
  
    // Havi sorok
    for (let i = 0; i < 12; i++) {
      const row = document.createElement("tr");
      row.innerHTML = `<th>${honapok[i]}</th>` + evek.map(ev => {
        const value = haviDijak[ev][i].toFixed(2) + " Ft";
        const cls = kedvezmenyesEvek.includes(parseInt(ev)) ? "table-success-col" : "";
        return `<td class="${cls}">${value}</td>`;
      }).join("");
      table.appendChild(row);
    }
  
    // Éves összeg
    const totalRow = document.createElement("tr");
    totalRow.innerHTML = `<th>Éves összesen</th>` + evek.map(ev => {
      const value = evesDijak[ev].toFixed(2) + " Ft";
      const cls = kedvezmenyesEvek.includes(parseInt(ev)) ? "table-success-col fw-bold" : "fw-bold";
      return `<td class="${cls}">${value}</td>`;
    }).join("");
    table.appendChild(totalRow);
  }
  
