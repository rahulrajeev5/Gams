Scalar
    budget,
    priceCar,
    priceTruck,
    revenueCar,
    revenueTruck,
    barnSpaces,
    truckSpaces;

$include ../GamsModels/input.inc

Integer Variables
    x1 "number of cars",
    x2 "number of trucks";

Free Variable
    z "total revenue";

Equations
    eq_revenue "revenue to be maximized",
    eq_budget "stay within budget",
    eq_carsinbarn "maximum number of cars in barn";

eq_revenue..       z =e= revenueCar * x1 + revenueTruck * x2;
eq_budget..        priceCar * x1 + priceTruck * x2 =l= budget;
eq_carsinbarn..    x1 =l= barnSpaces - x2;

x2.up = truckSpaces;

Model vintage /all/;
Solve vintage using mip maximizing z;

* Write results to CSV in backend folder
file results / '..\\GamsModels/result.csv' /;
put results;
put "car,", x1.l:0:0 /;
put "truck,", x2.l:0:0 /;
put "revenue,", z.l:0:0 /;
