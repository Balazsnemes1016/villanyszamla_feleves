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
        const response = await fetch("https://localhost:7171/api/villanyszamla/szamitas", {
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
  
  
