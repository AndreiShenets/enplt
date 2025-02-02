// k6 install:
//     winget install k6 --source winget
// Command to:
//     run k6 run --vus 10 --duration 30s k6-test.js

import http from 'k6/http';

const products = new Map();
products.set(0, ["Heatpumps"]);
products.set(1, ["SolarPanels"]);
products.set(2, ["Heatpumps", "SolarPanels"]);

const languages = [ "English", "German" ];
const customerRatings = ["Gold", "Silver", "Bronze"];

const start = new Date("2024-05-03").getTime();
const end = new Date(start + 5 * 365 * 24 * 60 * 60 * 1000).getTime();

function generateRandomDate() {
    return new Date(start + Math.random() * (end - start));
}

const params = {
    headers: {
        'Content-Type': 'application/json',
    },
};

const url = 'http://localhost:3000/calendar/query';

export default function () {
    const language = Math.floor(Math.random() * languages.length);
    const product = Math.floor(Math.random() * products.size);
    const rating = Math.floor(Math.random() * customerRatings.length);

    const payload =
        JSON.stringify(
            {
                date: generateRandomDate().toISOString().substring(0, 10),
                products: products.get(product),
                language: languages[language],
                rating: customerRatings[rating]
            }
        );

    http.post(url, payload, params);
}
