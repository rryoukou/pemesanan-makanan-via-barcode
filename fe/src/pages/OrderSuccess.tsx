import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

interface OrderDetailResponse {
  orderId: number;
  customerName: string;
  tableNumber: string;
  totalAmount: number;
  status: string;
  itemsBeli: Array<{          // <-- Pastikan namanya itemsBeli sesuai data API
    namaProduk: string;
    quantity: number;
    price: number;
    notes: string | null;
  }>;
}

export default function OrderSuccess() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [order, setOrder] = useState<OrderDetailResponse | null>(null);
  const [loading, setLoading] = useState(true);

  // Ambil data status pesanan terakhir dari backend untuk memastikan datanya masuk
  useEffect(() => {
    const fetchOrderData = async () => {
      try {
        // Kita gunakan endpoint GET /api/orders yang mencakup pencarian ID / nama
        const response = await fetch(`http://localhost:5029/api/orders?search=${id}`);
        if (response.ok) {
          const data = await response.json();
          // Karena data API berbentuk array list, kita ambil index ke-0
          if (data && data.length > 0) {
            setOrder(data[0]);
          }
        }
      } catch (error) {
        console.error("Gagal memuat status order:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchOrderData();
  }, [id]);

  if (loading) return <p>Sedang memuat struk pesanan...</p>;
  if (!order) return <p>Data pesanan tidak ditemukan di sistem server.</p>;

  return (
    <div style={{ textAlign: 'center', marginTop: '20px' }}>
      <div style={{ fontSize: '50px', color: '#10b981' }}>🎉</div>
      <h2 style={{ color: '#10b981', margin: '8px 0' }}>Pesanan Berhasil Dikirim!</h2>
      <p style={{ color: '#6b7280', margin: '0 0 20px 0' }}>Pesanan kamu sudah diteruskan ke dapur.</p>

      {/* Kartu Invoice Kecil */}
      <div style={{ background: '#f8fafc', border: '1px dashed #cbd5e1', borderRadius: '12px', padding: '16px', textAlign: 'left' }}>
        <h4 style={{ margin: '0 0 12px 0', borderBottom: '1px solid #e2e8f0', paddingBottom: '8px', textAlign: 'center' }}>
          STRUK PEMESANAN AKTIF
        </h4>
        <p style={{ margin: '4px 0', fontSize: '14px' }}><strong>ID Order:</strong> #{order.orderId}</p>
        <p style={{ margin: '4px 0', fontSize: '14px' }}><strong>Nama Pelanggan:</strong> {order.customerName}</p>
        <p style={{ margin: '4px 0', fontSize: '14px' }}><strong>Nomor Meja:</strong> Meja {order.tableNumber}</p>
        <p style={{ margin: '4px 0', fontSize: '14px' }}><strong>Status Pembayaran:</strong> <span style={{ color: '#e11d48', fontWeight: 'bold' }}>{order.status}</span></p>
        
        <hr style={{ border: '0', borderTop: '1px dotted #cbd5e1', margin: '12px 0' }} />

        {/* List Menu di Struk */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
          {order.itemsBeli?.map((item: any, idx: number) => (
            <div key={idx} style={{ display: 'flex', justifyContent: 'space-between', fontSize: '14px' }}>
              <span>{item.namaProduk} ({item.quantity}x)</span>
              <span>Rp {(item.price * item.quantity).toLocaleString('id-ID')}</span>
            </div>
          ))}
        </div>

        <hr style={{ border: '0', borderTop: '1px dashed #cbd5e1', margin: '12px 0' }} />
        
        <div style={{ display: 'flex', justifyContent: 'space-between', fontWeight: 'bold', fontSize: '16px' }}>
          <span>Total Tagihan:</span>
          <span style={{ color: '#10b981' }}>Rp {order.totalAmount.toLocaleString('id-ID')}</span>
        </div>
      </div>

      <button 
        onClick={() => navigate('/menu')}
        style={{
          marginTop: '24px',
          width: '100%',
          padding: '12px',
          background: '#1e293b',
          color: '#fff',
          border: 'none',
          borderRadius: '8px',
          fontWeight: 'bold',
          cursor: 'pointer'
        }}
      >
        Kembali ke Halaman Utama Menu
      </button>
    </div>
  );
}