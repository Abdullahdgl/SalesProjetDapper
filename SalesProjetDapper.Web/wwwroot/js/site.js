const SalesPulse = (() => {
    function para(n) {
        return new Intl.NumberFormat("tr-TR", {
            style: "currency",
            currency: "TRY",
            maximumFractionDigits: 0
        }).format(n || 0);
    }

    //-------------------
    function sayi(n) {
        return new Intl.NumberFormat("tr-TR").format(n || 0);
    }

    //-----------------------
    function TarihYazi(d) {
        const yil = d.getFullYear();
        const ay = String(d.getMonth() + 1).padStart(2, "0");
        const gun = String(d.getDate()).padStart(2, "0");
        return `${yil}-${ay}-${gun}`;
    }

    const cizimler = {};

    //----------------
    function ciz(id, ayar) {
        if (cizimler[id]) {
            cizimler[id].destroy();
        }
        const el = document.getElementById(id);
        if (!el) return;
        cizimler[id] = new Chart(el, ayar);

    }

    //-----------------------

    async function dashboard() {
        const fromEl = document.getElementById("fromDate");
        const toEl = document.getElementById("toDate");

        const bugun = new Date();
        const onİkiAyOnce = new Date(bugun.getFullYear(), bugun.getMonth() - 11, 1);
        if (fromEl && !fromEl.value) fromEl.value = TarihYazi(onİkiAyOnce);
        if (toEl && !toEl.value) toEl.value = TarihYazi(bugun);

        async function yukle() {
            const url = "/Dashboard/Data?from=" + fromEl.value + "&to=" + toEl.value;
            const cevap = await fetch(url);
            const data = await cevap.json();

            document.getElementById("kpiOrders").textContent = sayi(data.kpis.totalOrders);
            document.getElementById("kpiRevenue").textContent = para(data.kpis.totalRevenue);
            document.getElementById("kpiCustomers").textContent = sayi(data.kpis.totalCustomers);
            document.getElementById("kpiBasket").textContent = para(data.kpis.averageBasket);



            ciz("monthlyChart", {
                type: "line",
                data: {
                    labels: data.monthlySales.map(x => x.label),
                    datasets: [
                        { label: "Ciro", data: data.monthlySales.map(x => x.revenue) },
                        { label: "Sipariş", data: data.monthlySales.map(x => x.orderCount) }
                    ]
                }
            });

            ciz("categoryChart", {
                type: "bar",
                data: {
                    labels: data.topCategories.map(x => x.name),
                    datasets: [{ label: "Ciro", data: data.topCategories.map(x => x.revenue) }]
                }
            });


            ciz("cityChart", {

                type: "bar",
                data: {
                    labels: data.topCities.map(x => x.name),
                    datasets: [{ label: "Ciro", data: data.topCities.map(x => x.revenue) }]
                }
            });



            ciz("statusChart", {
                type: "doughnut",
                data: {
                    labels: data.statusBreakdown.map(x => x.status),
                    datasets: [{ data: data.statusBreakdown.map(x => x.count) }]
                }

            });
            document.getElementById("topProducts").innerHTML = data.topProducts.map(p =>
                "<tr><td>" + p.name + "</td><td>" + sayi(p.quantity) + "</td><td>" + para(p.revenue) + "</td></tr>"
            ).join("");
        }
        document.getElementById("applyRange").addEventListener("click", yukle);
        await yukle();
    }
    return { dashboard };  
})();