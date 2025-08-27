import logo from './logo.svg';
import './App.css';
import { useState } from 'react';

function App() {
  const [localSalesCount, setLocalSalesCount] = useState(0);
  const [foreignSalesCount, setForeignSalesCount] = useState(0);
  const [averageSaleAmount, setAverageSaleAmount] = useState(0);

  const [fcamaraCommission, setFcamaraCommission] = useState(null);
  const [competitorCommission, setCompetitorCommission] = useState(null);
  const [error, setError] = useState(null);

  async function calculate(e) {
    e.preventDefault(); // prevent page reload
    const apiBaseUrl = process.env.REACT_APP_API_BASE_URL;
    try {
      
      const response = await fetch(`${apiBaseUrl}/api/v1/commission`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          localSalesCount: parseInt(localSalesCount),
          foreignSalesCount: parseInt(foreignSalesCount),
          averageSaleAmount: parseFloat(averageSaleAmount)
        })
      });

      const contentType = response.headers.get('content-type');

      if (!response.ok) {
        if (response.status === 400 && contentType?.includes('application/problem+json')) {
          const problem = await response.json();
          const validationMessages = Object.entries(problem.errors)
            .map(([field, messages]) => `${field}: ${messages.join(', ')}`)
            .join('\n');

          setError(validationMessages);
        } else {
          const errorData = await response.json();
          setError(errorData.title || 'Calculation failed');
        }

        setFcamaraCommission(null);
        setCompetitorCommission(null);
        return;
      }

      const data = await response.json();

      setFcamaraCommission(data.fCamaraCommissionAmount);
      setCompetitorCommission(data.competitorCommissionAmount);
      setError(null);
    } catch (err) {
      setError('Failed to connect to the backend.');
      setFcamaraCommission(null);
      setCompetitorCommission(null);
    }
  }

  return (
    <div className="App">
      <header className="App-header">
        <form onSubmit={calculate}>
          <label htmlFor="localSalesCount">Local Sales Count</label>
          <input
            name="localSalesCount"
            type="number"
            value={localSalesCount}
            onChange={(e) => setLocalSalesCount(e.target.value)}
          /><br />

          <label htmlFor="foreignSalesCount">Foreign Sales Count</label>
          <input
            name="foreignSalesCount"
            type="number"
            value={foreignSalesCount}
            onChange={(e) => setForeignSalesCount(e.target.value)}
          /><br />

          <label htmlFor="averageSaleAmount">Average Sale Amount</label>
          <input
            name="averageSaleAmount"
            type="number"
            step="0.01"
            value={averageSaleAmount}
            onChange={(e) => setAverageSaleAmount(e.target.value)}
          /><br />

          <button type="submit">Calculate</button>
        </form>
      </header>

      <div>
        <h3>Results</h3>
        {error && (
          <p style={{ color: 'red', whiteSpace: 'pre-wrap' }}>
            Error: {error}
          </p>
        )}
        {fcamaraCommission !== null && (
          <>
            <p>Total FCamara commission: {fcamaraCommission}</p>
            <p>Total Competitor commission: {competitorCommission}</p>
          </>
        )}
      </div>
    </div>
  );
}

export default App;