// Function to generate and print a restaurant bill
window.generateBillPDF = function (jsonData) {
    const billData = JSON.parse(jsonData);

    // Format the current date and time
    const now = new Date();
    const formattedDate = now.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });

    // Create a printable bill HTML
    const billHtml = `
        <div class="bill-print" style="font-family: Arial, sans-serif; max-width: 800px; margin: 0 auto; padding: 20px;">
            <div style="text-align: center; margin-bottom: 20px;">
                <h1 style="color: #1a2a44; margin-bottom: 5px;">MASALADAR BITES</h1>
                <p style="color: #666; margin-top: 0;">123 Restaurant Street, Food City</p>
                <p style="color: #666; margin-top: 0;">Phone: (123) 456-7890</p>
                <p style="color: #666; margin-top: 5px; border-bottom: 2px solid #f5c542; padding-bottom: 10px;">Restaurant Bill</p>
            </div>
            
            <div style="display: flex; justify-content: space-between; margin-bottom: 20px;">
                <div>
                    <p><strong>Customer:</strong> ${billData.CustomerName || 'Walk-in Customer'}</p>
                    <p><strong>Table:</strong> ${billData.TableNumber || 'N/A'}</p>
                </div>
                <div style="text-align: right;">
                    <p><strong>Date:</strong> ${billData.Date || formattedDate}</p>
                    <p><strong>Bill No:</strong> ${billData.BillNumber || Math.floor(100000 + Math.random() * 900000)}</p>
                </div>
            </div>
            
            <table style="width: 100%; border-collapse: collapse; margin-bottom: 20px;">
                <thead>
                    <tr style="background-color: #f5c542; color: #1a2a44;">
                        <th style="padding: 10px; text-align: left; border-bottom: 2px solid #1a2a44;">Item</th>
                        <th style="padding: 10px; text-align: center; border-bottom: 2px solid #1a2a44;">Price</th>
                        <th style="padding: 10px; text-align: center; border-bottom: 2px solid #1a2a44;">Qty</th>
                        <th style="padding: 10px; text-align: right; border-bottom: 2px solid #1a2a44;">Total</th>
                    </tr>
                </thead>
                <tbody>
                    ${billData.Items.map(item => `
                        <tr>
                            <td style="padding: 10px; border-bottom: 1px solid #ddd;">${item.Name}</td>
                            <td style="padding: 10px; text-align: center; border-bottom: 1px solid #ddd;">$${item.Price.toFixed(2)}</td>
                            <td style="padding: 10px; text-align: center; border-bottom: 1px solid #ddd;">${item.Quantity}</td>
                            <td style="padding: 10px; text-align: right; border-bottom: 1px solid #ddd;">$${(item.Price * item.Quantity).toFixed(2)}</td>
                        </tr>
                    `).join('')}
                </tbody>
            </table>
            
            <div style="margin-left: auto; width: 300px; border-top: 2px solid #1a2a44; padding-top: 10px;">
                <div style="display: flex; justify-content: space-between;">
                    <span><strong>Subtotal:</strong></span>
                    <span><strong>$${billData.Subtotal.toFixed(2)}</strong></span>
                </div>
                ${billData.Discount > 0 ? `
                <div style="display: flex; justify-content: space-between;">
                    <span>Discount (${billData.Discount}%):</span>
                    <span>-$${billData.DiscountAmount.toFixed(2)}</span>
                </div>
                ` : ''}
                ${billData.Tax > 0 ? `
                <div style="display: flex; justify-content: space-between;">
                    <span>Tax (${billData.Tax}%):</span>
                    <span>$${billData.TaxAmount.toFixed(2)}</span>
                </div>
                ` : ''}
                ${billData.ServiceCharge > 0 ? `
                <div style="display: flex; justify-content: space-between;">
                    <span>Service Charge (${billData.ServiceCharge}%):</span>
                    <span>$${billData.ServiceChargeAmount.toFixed(2)}</span>
                </div>
                ` : ''}
                <div style="display: flex; justify-content: space-between; font-weight: bold; font-size: 1.2em; margin-top: 10px; border-top: 1px dashed #1a2a44; padding-top: 10px;">
                    <span>TOTAL:</span>
                    <span>$${billData.Total.toFixed(2)}</span>
                </div>
            </div>
            
            <div style="margin-top: 30px; text-align: center; color: #666; font-size: 0.9em; border-top: 2px solid #f5c542; padding-top: 20px;">
                <p>Thank you for dining with us!</p>
                <p>Please visit again</p>
                <p style="margin-top: 10px;">✆ (123) 456-7890 | ✉ info@masaladarbites.com</p>
            </div>
        </div>
    `;

    // Create a new window for printing
    const printWindow = window.open('', '_blank', 'width=800,height=600');
    printWindow.document.open();
    printWindow.document.write(`
        <html>
            <head>
                <title>Bill - ${billData.BillNumber || 'Order'}</title>
                <style>
                    @media print {
                        body { 
                            margin: 0; 
                            padding: 0; 
                            -webkit-print-color-adjust: exact !important;
                            print-color-adjust: exact !important;
                        }
                        .bill-print { 
                            max-width: 100% !important; 
                            padding: 10px !important; 
                            box-shadow: none !important;
                        }
                        .no-print { 
                            display: none !important; 
                        }
                        @page { 
                            size: auto; 
                            margin: 5mm; 
                        }
                    }
                    @media screen {
                        body { 
                            background: #f5f5f5; 
                            padding: 20px; 
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            min-height: 100vh;
                        }
                    }
                    .print-actions {
                        display: flex;
                        justify-content: center;
                        gap: 10px;
                        margin: 20px auto;
                        flex-wrap: wrap;
                    }
                    button {
                        background: #1a2a44;
                        color: white;
                        border: none;
                        padding: 10px 20px;
                        cursor: pointer;
                        border-radius: 4px;
                        transition: all 0.3s ease;
                    }
                    button:hover {
                        background: #f5c542;
                        color: #1a2a44;
                        transform: translateY(-2px);
                    }
                    button.print-btn {
                        background: #28a745;
                    }
                    button.print-btn:hover {
                        background: #218838;
                        color: white;
                    }
                </style>
            </head>
            <body>
                ${billHtml}
                <div class="print-actions no-print">
                    <button class="print-btn" onclick="window.print()">
                        <i class="bi bi-printer"></i> Print Bill
                    </button>
                    <button onclick="window.close()">
                        <i class="bi bi-x-circle"></i> Close
                    </button>
                </div>
                <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.8.0/font/bootstrap-icons.css">
            </body>
        </html>
    `);
    printWindow.document.close();

    // Focus on the print button for better UX
    printWindow.addEventListener('load', function () {
        const printBtn = printWindow.document.querySelector('.print-btn');
        if (printBtn) printBtn.focus();
    });
};