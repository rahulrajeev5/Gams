import React, { useState } from 'react';
import axios from 'axios';
import '../styles/App.css'; 

interface OptimizationInput {
  budget: number;
  priceCar: number;
  priceTruck: number;
  revenueCar: number;
  revenueTruck: number;
  barnSpaces: number;
  truckSpaces: number;
}

interface OptimizationResult {
  cars: number;
  trucks: number;
  revenue: number;
}

const OptimizationForm: React.FC = () => {
  const [input, setInput] = useState<OptimizationInput>({
    budget: 3.6,
    priceCar: 1,
    priceTruck: 1.2,
    revenueCar: 2,
    revenueTruck: 2.5,
    barnSpaces: 4,
    truckSpaces: 2,
  });

  const [result, setResult] = useState<OptimizationResult | null>(null);
  const [error, setError] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setInput({ ...input, [e.target.name]: parseFloat(e.target.value) });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setResult(null);

    try {
      const res = await axios.post<OptimizationResult>(
        'http://localhost:5000/api/optimization/run',
        input
      );
      setResult(res.data);
    } catch (err: any) {
      setError(err.response?.data || 'Something went wrong');
    }
  };
  
  const formatLabel = (key: string) => {
    return key
      .replace(/([a-z])([A-Z])/g, '$1 $2') // Adds space before capital letters
      .replace(/([A-Z])/g, ' $1')          // Handles acronyms, optional
      .replace(/^./, str => str.toUpperCase()); // Capitalizes first letter
  };
  

  return (
    <div className="container">
      <h1>GAMS Optimization</h1>
      <form onSubmit={handleSubmit}>
        {Object.entries(input).map(([key, value]) => (
          <div key={key} className="form-group">
            <label htmlFor={key}>{formatLabel(key)}:</label>
            <input
              id={key}
              type="number"
              name={key}
              value={value}
              onChange={handleChange}
              step="any"
            />
          </div>
        ))}
        <button type="submit">Run Optimization</button>
      </form>

      {error && <p className="error">❌ {error}</p>}

      {result && (
        <div className="result">
          <h3>Result</h3>
          <p>Cars: {result.cars}</p>
          <p>Trucks: {result.trucks}</p>
          <p>Revenue: {result.revenue}</p>
        </div>
      )}
    </div>
  );
};

export default OptimizationForm;
