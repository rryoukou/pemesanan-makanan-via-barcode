import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

interface RecentOrder {
  orderId: number;
  tableNumber: string;
  customerName: string;
  totalAmount: number;
  status: string;
  orderDate: string;
}

interface ChartData {
  date: string;
  revenue: number;
}

interface DashboardStats {
  totalRevenue: number;
  totalOrders: number;
  productSold: number;
  recentOrders: RecentOrder[];
  revenueChart: ChartData[];
}

export default function AdminDashboard() {
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  // 1. Ambil data statistik dashboard dari backend C#
  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        const response = await fetch('https://3254jhsj-5029.asse.devtunnels.ms/api/dashboard/stats');
        if (response.ok) {
          const data = await response.json();
          setStats(data);
        }
      } catch (error) {
        console.error("Gagal memuat data statistik dashboard:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchDashboardData();
  }, []);

  if (loading) return <p>Sedang memuat data dashboard admin...</p>;
  if (!stats) return <p>Gagal memeriksa statistik dari server.</p>;

  // Cari nilai pendapatan tertinggi untuk kalkulasi skala tinggi grafik bar CSS
  const maxRevenue = stats.revenueChart.length > 0 
    ? Math.max(...stats.revenueChart.map(d => d.revenue)) 
    : 100000;

  return (
    <div style={{ fontFamily: 'sans-serif', color: '#1e293b' }}>
      {/* 2. HEADER & MENU NAVIGASI ADMIN */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', borderBottom: '2px solid #e2e8f0', paddingBottom: '16px', marginBottom: '24px' }}>
        <h2 style={{ margin: 0, color: '#e11d48' }}>Dashboard Utama Resto</h2>
        <div style={{ display: 'flex', gap: '12px' }}>
          <button onClick={() => navigate('/admin/dashboard')} style={{ padding: '8px 16px', background: '#e11d48', color: '#fff', border: 'none', borderRadius: '6px', fontWeight: 'bold', cursor: 'pointer' }}>Dashboard</button>
          <button onClick={() => navigate('/admin/orders')} style={{ padding: '8px 16px', background: '#f1f5f9', border: '1px solid #cbd5e1', borderRadius: '6px', cursor: 'pointer' }}>Orders</button>
          <button onClick={() => navigate('/admin/inventory')} style={{ padding: '8px 16px', background: '#f1f5f9', border: '1px solid #cbd5e1', borderRadius: '6px', cursor: 'pointer' }}>Inventory</button>
          <button onClick={() => navigate('/menu')} style={{ padding: '8px 16px', background: '#334155', color: '#fff', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>Keluar</button>
        </div>
      </div>

      {/* 3. KARTU STATISTIK RINGKASAN (METRICS CARD) */}
      <div style={{ display: 'flex', gap: '16px', marginBottom: '32px', flexWrap: 'wrap' }}>
        <div style={{ flex: '1', minWidth: '200px', background: '#f8fafc', border: '1px solid #e2e8f0', padding: '20px', borderRadius: '12px', boxShadow: '0 1px 3px rgba(0,0,0,0.05)' }}>
          <p style={{ margin: '0 0 8px 0', fontSize: '14px', color: '#64748b', fontWeight: 'bold' }}>TOTAL VENUE (PENDAPATAN)</p>
          <h2 style={{ margin: 0, color: '#10b981' }}>Rp {stats.totalRevenue.toLocaleString('id-ID')}</h2>
        </div>
        <div style={{ flex: '1', minWidth: '200px', background: '#f8fafc', border: '1px solid #e2e8f0', padding: '20px', borderRadius: '12px', boxShadow: '0 1px 3px rgba(0,0,0,0.05)' }}>
          <p style={{ margin: '0 0 8px 0', fontSize: '14px', color: '#64748b', fontWeight: 'bold' }}>TOTAL ORDERS</p>
          <h2 style={{ margin: 0, color: '#3b82f6' }}>{stats.totalOrders} Pesanan</h2>
        </div>
        <div style={{ flex: '1', minWidth: '200px', background: '#f8fafc', border: '1px solid #e2e8f0', padding: '20px', borderRadius: '12px', boxShadow: '0 1px 3px rgba(0,0,0,0.05)' }}>
          <p style={{ margin: '0 0 8px 0', fontSize: '14px', color: '#64748b', fontWeight: 'bold' }}>PRODUCT SOLD (TERJUAL)</p>
          <h2 style={{ margin: 0, color: '#f59e0b' }}>{stats.productSold} Item Makanan</h2>
        </div>
      </div>

      {/* 4. GRAFIK PENDAPATAN HARIAN (VISUALISASI BAR) */}
      <div style={{ background: '#fff', border: '1px solid #e2e8f0', padding: '20px', borderRadius: '12px', marginBottom: '32px' }}>
        <h3 style={{ margin: '0 0 20px 0' }}>Grafik Pendapatan Toko</h3>
        {stats.revenueChart.length === 0 ? (
          <p style={{ color: '#94a3b8', fontSize: '14px' }}>Belum ada data transaksi lunas untuk membuat grafik.</p>
        ) : (
          <div style={{ display: 'flex', alignItems: 'flex-end', gap: '24px', height: '200px', paddingLeft: '16px', borderBottom: '2px solid #cbd5e1' }}>
            {stats.revenueChart.map((data, idx) => {
              // Hitung persen tinggi batang grafik secara dinamis
              const barHeight = (data.revenue / maxRevenue) * 150 + 20;
              return (
                <div key={idx} style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', flex: 1 }}>
                  <span style={{ fontSize: '11px', fontWeight: 'bold', color: '#10b981', marginBottom: '4px' }}>
                    {data.revenue > 0 ? `Rp ${data.revenue.toLocaleString('id-ID')}` : '0'}
                  </span>
                  <div style={{ width: '100%', maxWidth: '60px', height: `${barHeight}px`, background: 'linear-gradient(to top, #10b981, #34d399)', borderRadius: '6px 6px 0 0' }}></div>
                  <span style={{ fontSize: '12px', color: '#64748b', marginTop: '8px' }}>{data.date}</span>
                </div>
              );
            })}
          </div>
        )}
      </div>

      {/* 5. RECENT ORDERS (TABEL PESANAN TERBARU) */}
      <div style={{ background: '#fff', border: '1px solid #e2e8f0', padding: '20px', borderRadius: '12px' }}>
        <h3 style={{ margin: '0 0 16px 0' }}>Recent Orders (Pesanan Terbaru)</h3>
        {stats.recentOrders.length === 0 ? (
          <p style={{ color: '#94a3b8', fontSize: '14px' }}>Belum ada pesanan masuk hari ini.</p>
        ) : (
          <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left', fontSize: '14px' }}>
            <thead>
              <tr style={{ borderBottom: '2px solid #e2e8f0', background: '#f8fafc' }}>
                <th style={{ padding: '12px' }}>ID Order</th>
                <th style={{ padding: '12px' }}>Meja</th>
                <th style={{ padding: '12px' }}>Nama Pelanggan</th>
                <th style={{ padding: '12px' }}>Total Biaya</th>
                <th style={{ padding: '12px' }}>Status Pembayaran</th>
              </tr>
            </thead>
            <tbody>
              {stats.recentOrders.map((order) => (
                <tr key={order.orderId} style={{ borderBottom: '1px solid #f1f5f9' }}>
                  <td style={{ padding: '12px', fontWeight: 'bold' }}>#{order.orderId}</td>
                  <td style={{ padding: '12px' }}>Meja {order.tableNumber}</td>
                  <td style={{ padding: '12px' }}>{order.customerName}</td>
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
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}