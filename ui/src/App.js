import React, { useState } from "react";
import "./App.css";

function App() {
  const [localSalesCount, setLocalSalesCount] = useState("");
  const [foreignSalesCount, setForeignSalesCount] = useState("");
  const [averageSaleAmount, setAverageSaleAmount] = useState("");
  const [fcamaraCommission, setFcamaraCommission] = useState(0);
  const [competitorCommission, setCompetitorCommission] = useState(0);
  const [error, setError] = useState("");

  const calculate = async (event) => {
    event.preventDefault();
    setError("");

    if (!localSalesCount || !foreignSalesCount || !averageSaleAmount) {
      setError("Please fill in all fields");
      return;
    }

    try {
      const myHeaders = new Headers();
      myHeaders.append("Content-Type", "application/json");

      const config = {
        method: "POST",
        headers: myHeaders,
        body: JSON.stringify({
          localSalesCount: parseInt(localSalesCount),
          foreignSalesCount: parseInt(foreignSalesCount),
          averageSaleAmount: parseFloat(averageSaleAmount),
        }),
        redirect: "follow",
      };

      const response = await fetch(
        "https://localhost:5000/Commission/",
        config
      );

      if (!response.ok) {
        throw new Error("Calculation failed");
      }

      const data = await response.json();
      setFcamaraCommission(data.fCamaraCommissionAmount);
      setCompetitorCommission(data.competitorCommissionAmount);
    } catch (err) {
      setError("Error calculating commissions");
      console.error("Calculation error:", err);
    }
  };

  return (
    <div className="App">
      <header className="App-header">
        <h1>Commission Calculator</h1>
        <form onSubmit={calculate}>
          <div>
            <label htmlFor="localSalesCount">Local Sales Count</label>
            <input
              id="localSalesCount"
              type="number"
              value={localSalesCount}
              onChange={(e) => setLocalSalesCount(e.target.value)}
              min="0"
              required
            />
          </div>

          <div>
            <label htmlFor="foreignSalesCount">Foreign Sales Count</label>
            <input
              id="foreignSalesCount"
              type="number"
              value={foreignSalesCount}
              onChange={(e) => setForeignSalesCount(e.target.value)}
              min="0"
              required
            />
          </div>

          <div>
            <label htmlFor="averageSaleAmount">Average Sale Amount</label>
            <input
              id="averageSaleAmount"
              type="number"
              value={averageSaleAmount}
              onChange={(e) => setAverageSaleAmount(e.target.value)}
              min="0"
              step="0.01"
              required
            />
          </div>

          <button type="submit">Calculate</button>
        </form>

        {error && <div style={{ color: "red" }}>{error}</div>}

        <div>
          <h3>Results</h3>
          <p>Total FCamara Commission: £{fcamaraCommission.toFixed(2)}</p>
          <p>Total Competitor Commission: £{competitorCommission.toFixed(2)}</p>
        </div>
      </header>
    </div>
  );
}

export default App;
