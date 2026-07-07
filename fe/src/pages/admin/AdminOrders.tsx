import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

interface OrderItem {
  namaProduk: string;
  quantity: number;
  price: number;
  notes: string | null;
}

interface OrderData {
  orderId: number;
  tableNumber: string;
  customerName: string;
  orderDate: string;
  totalAmount: number;
  status: string;
  additionalNotes: string | null;
  itemsBeli: OrderItem[];
}

export default function AdminOrders() {
  const [orders, setOrders] = useState<OrderData[]>([]);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(true);
  const [selectedReceipt, setSelectedReceipt] = useState<any | null>(null);
  const navigate = useNavigate();

  // 1. Ambil data orders dari API Backend C#
  const fetchOrders = async (keyword: string) => {
    try {
      setLoading(true);
      const url = keyword 
        ? `http://localhost:5029/api/orders?search=${keyword}`
        : `http://localhost:5029/api/orders`;
      const response = await fetch(url);
      const data = await response.json();
      setOrders(data);
    } catch (error) {
      console.error("Gagal mengambil data orderan:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchOrders(search);
  }, [search]);

  // 2. Fungsi Aksi: Mengubah Status Menjadi Lunas
  const handleMarkAsLunas = async (orderId: number) => {
    try {
      const response = await fetch(`http://localhost:5029/api/orders/${orderId}/lunas`, {
        method: 'PUT'
      });
      if (response.ok) {
        alert("Pesanan berhasil ditandai LUNAS!");
        fetchOrders(search); // Refresh data tabel harian
      }
    } catch (error) {
      console.error("Gagal mengubah status:", error);
    }
  };

  // 3. Fungsi Aksi: Ambil Data Struk Pembelian
  const handlePrintReceipt = async (orderId: number) => {
    try {
      const response = await fetch(`http://localhost:5029/api/orders/${orderId}/struk`);
      const data = await response.json();
      if (response.ok) {
        setSelectedReceipt(data);
      } else {
        alert(data.message || "Gagal membuat struk.");
      }
    } catch (error) {
      console.error("Gagal mengambil data struk:", error);
    }
  };

  return (
    <div style={{ fontFamily: 'sans-serif', color: '#1e293b' }}>
      {/* NAVIGATION BAR ADMIN */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', borderBottom: '2px solid #e2e8f0', paddingBottom: '16px', marginBottom: '24px' }}>
        <h2 style={{ margin: 0, color: '#e11d48' }}>Manajemen Pesanan Resto</h2>
        <div style={{ display: 'flex', gap: '12px' }}>
          <button onClick={() => navigate('/admin/dashboard')} style={{ padding: '8px 16px', background: '#f1f5f9', border: '1px solid #cbd5e1', borderRadius: '6px', cursor: 'pointer' }}>Dashboard</button>
          <button onClick={() => navigate('/admin/orders')} style={{ padding: '8px 16px', background: '#e11d48', color: '#fff', border: 'none', borderRadius: '6px', fontWeight: 'bold', cursor: 'pointer' }}>Orders</button>
          <button onClick={() => navigate('/admin/inventory')} style={{ padding: '8px 16px', background: '#f1f5f9', border: '1px solid #cbd5e1', borderRadius: '6px', cursor: 'pointer' }}>Inventory</button>
          <button onClick={() => navigate('/menu')} style={{ padding: '8px 16px', background: '#334155', color: '#fff', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>Keluar</button>
        </div>
      </div>

      {/* INPUT PENCARIAN ORDER */}
      <input
        type="text"
        placeholder="Cari berdasarkan nama customer atau nomor meja..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        style={{ width: '100%', padding: '12px', borderRadius: '8px', border: '1px solid #ccc', marginBottom: '20px', boxSizing: 'border-box' }}
      />

      {loading ? (
        <p>Sedang memuat data orderan...</p>
      ) : orders.length === 0 ? (
        <p>Tidak ada orderan masuk yang cocok dengan pencarian.</p>
      ) : (
        <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left', fontSize: '14px' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid #e2e8f0', background: '#f8fafc' }}>
              <th style={{ padding: '12px' }}>Order ID</th>
              <th style={{ padding: '12px' }}>Meja</th>
              <th style={{ padding: '12px' }}>Nama Customer</th>
              <th style={{ padding: '12px' }}>Pesanan Yang Dibeli</th>
              <th style={{ padding: '12px' }}>Harga Pembelian</th>
              <th style={{ padding: '12px' }}>Status</th>
              <th style={{ padding: '12px' }}>Action</th>
            </tr>
          </thead>
          <tbody>
            {orders.map((order) => (
              <tr key={order.orderId} style={{ borderBottom: '1px solid #f1f5f9' }}>
                <td style={{ padding: '12px', fontWeight: 'bold' }}>#{order.orderId}</td>
                <td style={{ padding: '12px' }}>Meja {order.tableNumber}</td>
                <td style={{ padding: '12px' }}>{order.customerName}</td>
                <td style={{ padding: '12px' }}>
                  {order.itemsBeli.map((item, idx) => (
                    <div key={idx} style={{ fontSize: '13px', marginBottom: '4px' }}>
                      • {item.namaProduk} <strong>({item.quantity}x)</strong>
                      {item.notes && <span style={{ color: '#e11d48', fontSize: '11px', display: 'block', paddingLeft: '8px' }}>↳ "{item.notes}"</span>}
                    </div>
                  ))}
                </td>
                <td style={{ padding: '12px', fontWeight: 'bold', color: '#10b981' }}>Rp {order.totalAmount.toLocaleString('id-ID')}</td>
                <td style={{ padding: '12px' }}>
                  <span style={{
                    padding: '4px 10px',
                    borderRadius: '20px',
                    fontSize: '12px',
                    fontWeight: 'bold',
                    backgroundColor: order.status === 'Lunas' ? '#dcfce7' : '#fee2e2',
                    color: order.status === 'Lunas' ? '#15803d' : '#b91c1c'
                  }}>
                    {order.status}
                  </span>
                </td>
                <td style={{ padding: '12px' }}>
                  <div style={{ display: 'flex', gap: '8px' }}>
                    {order.status === 'Belum Bayar' && (
                      <button onClick={() => handleMarkAsLunas(order.orderId)} style={{ padding: '6px 10px', background: '#10b981', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '12px', fontWeight: 'bold' }}>
                        Mark as Lunas
                      </button>
                    )}
                    <button onClick={() => handlePrintReceipt(order.orderId)} disabled={order.status !== 'Lunas'} style={{ padding: '6px 10px', background: order.status === 'Lunas' ? '#3b82f6' : '#94a3b8', color: '#fff', border: 'none', borderRadius: '4px', cursor: order.status === 'Lunas' ? 'pointer' : 'not-allowed', fontSize: '12px', fontWeight: 'bold' }}>
                      Buat Struk
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {/* MODAL POPUP PREVIEW STRUK PEMBELIAN */}
      {selectedReceipt && (
        <div style={{ position: 'fixed', top: 0, left: 0, width: '100%', height: '100%', background: 'rgba(0,0,0,0.5)', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
          <div style={{ background: '#fff', padding: '24px', borderRadius: '8px', width: '100%', maxWidth: '36px', minWidth: '320px', color: '#000', fontFamily: 'monospace', boxShadow: '0 4px 6px rgba(0,0,0,0.1)' }}>
            <h3 style={{ textTransform: 'uppercase', textAlign: 'center', margin: '0 0 4px 0' }}>{selectedReceipt.namaResto}</h3>
            <p style={{ textAlign: 'center', margin: '0 0 16px 0', fontSize: '12px' }}>{selectedReceipt.alamat}</p>
            <p style={{ margin: '4px 0' }}>Nota: {selectedReceipt.noNota}</p>
            <p style={{ margin: '4px 0' }}>Meja: {selectedReceipt.meja} | Cust: {selectedReceipt.pelanggan}</p>
            <p style={{ margin: '4px 0', fontSize: '12px', color: '#666' }}>Waktu: {selectedReceipt.waktuCetak}</p>
            <hr style={{ border: 'none', borderTop: '1px dashed #000', margin: '12px 0' }} />
            
            {selectedReceipt.itemBelanja.map((item: any, idx: number) => (
              <div key={idx} style={{ marginBottom: '8px' }}>
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                  <span>{item.menu}</span>
                  <span>Rp {item.subTotal.toLocaleString('id-ID')}</span>
                </div>
                <div style={{ fontSize: '12px', color: '#555' }}>{item.quantity} x Rp {item.hargaSatuan.toLocaleString('id-ID')}</div>
              </div>
            ))}

            <hr style={{ border: 'none', borderTop: '1px dashed #000', margin: '12px 0' }} />
            <div style={{ display: 'flex', justifyContent: 'space-between', fontWeight: 'bold', fontSize: '16px' }}>
              <span>TOTAL:</span>
              <span>Rp {selectedReceipt.totalBayar.toLocaleString('id-ID')}</span>
            </div>
            <p style={{ textAlign: 'center', marginTop: '20px', fontSize: '12px', fontWeight: 'bold', color: '#15803d' }}>--- LUNAS ---</p>
            <button onClick={() => setSelectedReceipt(null)} style={{ width: '100%', marginTop: '16px', padding: '8px', background: '#e11d48', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontFamily: 'sans-serif', fontWeight: 'bold' }}>Tutup</button>
          </div>
        </div>
      )}
    </div>
  );
}