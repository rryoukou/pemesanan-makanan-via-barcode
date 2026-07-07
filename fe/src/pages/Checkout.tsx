import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

interface CartItem {
  productId: number;
  productName: string;
  price: number;
  quantity: number;
  notes: string;
}

export default function Checkout() {
  const [cart, setCart] = useState<CartItem[]>([]);
  const [customerName, setCustomerName] = useState('');
  const [additionalNotes, setAdditionalNotes] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const tableNumber = localStorage.getItem('table_number') || "01";

  // 1. Ambil data keranjang dari LocalStorage saat halaman dibuka
  useEffect(() => {
    const savedCart = JSON.parse(localStorage.getItem('cart') || '[]');
    setCart(savedCart);
  }, []);

  // 2. Fungsi untuk menambah / mengurangi kuantitas di halaman checkout
  const updateQuantity = (index: number, delta: number) => {
    const updatedCart = [...cart];
    updatedCart[index].quantity += delta;

    // Jika kuantitas menjadi 0, hapus item dari keranjang
    if (updatedCart[index].quantity <= 0) {
      updatedCart.splice(index, 1);
    }

    setCart(updatedCart);
    localStorage.setItem('cart', JSON.stringify(updatedCart));
  };

  // 3. Hitung Total Harga Pembelian
  const totalAmount = cart.reduce((sum, item) => sum + item.price * item.quantity, 0);

  // 4. Fungsi Kirim Data ke API C# (Kirim Pesanan)
  const handlePlaceOrder = async () => {
    if (!customerName.trim()) {
      alert("Mohon isi Nama kamu terlebih dahulu!");
      return;
    }
    if (cart.length === 0) {
      alert("Keranjang belanja kamu kosong.");
      return;
    }

    setLoading(false);
    
    // Sesuaikan dengan struktur OrderCreateDto di Backend C# kita
    const orderData = {
      tableNumber: tableNumber,
      customerName: customerName,
      additionalNotes: additionalNotes,
      cartItems: cart.map(item => ({
        productId: item.productId,
        quantity: item.quantity,
        notes: item.notes
      }))
    };

    try {
      setLoading(true);
      const response = await fetch('http://localhost:5029/api/orders', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(orderData)
      });

      const result = await response.json();

      if (response.ok) {
        // Kosongkan keranjang setelah berhasil checkout
        localStorage.removeItem('cart');
        // Arahkan ke halaman sukses membawa ID Order dari backend
        navigate(`/order-success/${result.orderId}`);
      } else {
        alert(result.message || "Gagal memproses pesanan.");
      }
    } catch (error) {
      console.error("Error saat checkout:", error);
      alert("Terjadi kesalahan jaringan dengan server.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <button onClick={() => navigate('/menu')} style={{ background: 'none', border: 'none', cursor: 'pointer', marginBottom: '16px', color: '#6b7280' }}>
        ← Tambah Menu Lagi
      </button>

      <h2>Ringkasan Pesanan</h2>
      <p style={{ fontSize: '14px', color: '#6b7280' }}>Meja Nomor: <strong>{tableNumber}</strong></p>

      {/* List Item yang Dibeli */}
      {cart.length === 0 ? (
        <p>Keranjang kamu kosong, silakan pilih menu terlebih dahulu.</p>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '12px', marginBottom: '24px' }}>
          {cart.map((item, index) => (
            <div key={index} style={{ borderBottom: '1px solid #e5e7eb', paddingBottom: '12px' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', fontWeight: 'bold' }}>
                <span>{item.productName}</span>
                <span>Rp {(item.price * item.quantity).toLocaleString('id-ID')}</span>
              </div>
              {item.notes && <p style={{ margin: '4px 0', fontSize: '12px', color: '#e11d48', fontStyle: 'italic' }}>✍️ "{item.notes}"</p>}
              
              {/* Tombol Aksi Edit Kuantitas */}
              <div style={{ display: 'flex', alignItems: 'center', gap: '8px', marginTop: '8px' }}>
                <button onClick={() => updateQuantity(index, -1)} style={{ padding: '2px 8px', cursor: 'pointer' }}>-</button>
                <span style={{ fontSize: '14px' }}>{item.quantity}x</span>
                <button onClick={() => updateQuantity(index, 1)} style={{ padding: '2px 8px', cursor: 'pointer' }}>+</button>
              </div>
            </div>
          ))}

          <div style={{ display: 'flex', justifyContent: 'space-between', fontWeight: 'bold', fontSize: '18px', marginTop: '12px', color: '#10b981' }}>
            <span>Total Pembayaran:</span>
            <span>Rp {totalAmount.toLocaleString('id-ID')}</span>
          </div>
        </div>
      )}

      <hr style={{ border: '0', borderTop: '1px solid #e5e7eb', margin: '20px 0' }} />

      {/* Formulir Informasi Pelanggan */}
      <div style={{ marginBottom: '16px' }}>
        <label style={{ fontWeight: 'bold', display: 'block', marginBottom: '6px', fontSize: '14px' }}>Nama Atas Nama Pesanan *</label>
        <input 
          type="text" 
          placeholder="Masukkan nama kamu..." 
          value={customerName}
          onChange={(e) => setCustomerName(e.target.value)}
          style={{ width: '100%', padding: '10px', borderRadius: '6px', border: '1px solid #ccc', boxSizing: 'border-box' }}
        />
      </div>

      <div style={{ marginBottom: '24px' }}>
        <label style={{ fontWeight: 'bold', display: 'block', marginBottom: '6px', fontSize: '14px' }}>Catatan Tambahan untuk Orderan</label>
        <textarea 
          placeholder="Contoh: Sendoknya minta 2 ya, atau tissue yang banyak" 
          value={additionalNotes}
          onChange={(e) => setAdditionalNotes(e.target.value)}
          rows={3}
          style={{ width: '100%', padding: '10px', borderRadius: '6px', border: '1px solid #ccc', boxSizing: 'border-box', fontFamily: 'sans-serif' }}
        />
      </div>

      <button 
        onClick={handlePlaceOrder}
        disabled={loading}
        style={{
          width: '100%',
          padding: '14px',
          background: '#10b981',
          color: '#fff',
          border: 'none',
          borderRadius: '8px',
          fontWeight: 'bold',
          fontSize: '16px',
          cursor: loading ? 'not-allowed' : 'pointer',
          opacity: loading ? 0.7 : 1
        }}
      >
        {loading ? "Sedang Memproses..." : "Kirim Pesanan Sekarang"}
      </button>
    </div>
  );
}