import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

interface Product {
  productId: number;
  productName: string;
  category: string;
  price: number;
  description: string;
  imageUrl: string;
  isAvailable: boolean;
}

export default function Menu() {
  const [products, setProducts] = useState<Product[]>([]);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(true);
  const [cartTotal, setCartTotal] = useState(0);
  const [cartCount, setCartCount] = useState(0);
  const navigate = useNavigate();
  
  const nomorMeja = localStorage.getItem('table_number') || "01"; // Default ke meja 01 jika tes tanpa barcode

  const fetchProducts = async (keyword: string) => {
    try {
      setLoading(true);
      const url = keyword 
        ? `https://3254jhsj-5029.asse.devtunnels.ms/api/products?search=${keyword}`
        : `https://3254jhsj-5029.asse.devtunnels.ms/api/products`;
        
      const response = await fetch(url);
      const data = await response.json();
      setProducts(data);
    } catch (error) {
      console.error("Gagal mengambil data menu:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const delayDebounce = setTimeout(() => {
      fetchProducts(search);
    }, 500);
    return () => clearTimeout(delayDebounce);
  }, [search]);

  // Hitung total harga dan jumlah item di keranjang secara realtime dari LocalStorage
  useEffect(() => {
    const cart = JSON.parse(localStorage.getItem('cart') || '[]');
    const totalHarga = cart.reduce((sum: number, item: any) => sum + (item.price * item.quantity), 0);
    const totalItem = cart.reduce((sum: number, item: any) => sum + item.quantity, 0);
    setCartTotal(totalHarga);
    setCartCount(totalItem);
  }, []);

  return (
    <div style={{ paddingBottom: '80px' }}> {/* Beri padding bawah agar konten tidak tertutup floating button */}
      <div style={{ marginBottom: '16px', background: '#f3f4f6', padding: '12px', borderRadius: '8px' }}>
        <h4 style={{ margin: 0 }}>📍 Nomor Meja: <span style={{ color: '#e11d48' }}>{nomorMeja}</span></h4>
      </div>

      <h2>Daftar Menu Makanan</h2>
      
      <input
        type="text"
        placeholder="Cari makanan atau minuman..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        style={{ width: '100%', padding: '10px', borderRadius: '6px', border: '1px solid #ccc', marginBottom: '20px', boxSizing: 'border-box' }}
      />

      {loading ? (
        <p>Sedang memuat menu...</p>
      ) : products.length === 0 ? (
        <p>Menu tidak ditemukan atau database masih kosong.</p>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
          {products.map((item) => (
            <div 
              key={item.productId} 
              onClick={() => navigate(`/menu/${item.productId}`)}
              style={{ display: 'flex', border: '1px solid #e5e7eb', borderRadius: '8px', padding: '12px', cursor: 'pointer', backgroundColor: '#fff', boxShadow: '0 1px 3px rgba(0,0,0,0.1)' }}
            >
              <img 
                src={item.imageUrl || 'https://placehold.co/100x100?text=Food'} 
                alt={item.productName} 
                style={{ width: '80px', height: '80px', borderRadius: '6px', objectFit: 'cover', marginRight: '12px' }}
              />
              <div style={{ flex: 1 }}>
                <h4 style={{ margin: '0 0 4px 0' }}>{item.productName}</h4>
                <p style={{ margin: '0 0 8px 0', fontSize: '12px', color: '#6b7280' }}>{item.description}</p>
                <span style={{ fontWeight: 'bold', color: '#10b981' }}>Rp {item.price.toLocaleString('id-ID')}</span>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* FLOATING CHECKOUT BUTTON BAR */}
      {cartCount > 0 && (
        <div style={{
          position: 'fixed',
          bottom: '16px',
          left: '50%',
          transform: 'translateX(-50%)',
          width: '90%',
          maxWidth: '440px',
          background: '#1e293b',
          borderRadius: '12px',
          padding: '12px 20px',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          boxShadow: '0 4px 10px rgba(0,0,0,0.3)',
          color: '#fff',
          boxSizing: 'border-box'
        }}>
          <div>
            <p style={{ margin: 0, fontSize: '12px', color: '#94a3b8' }}>Total Pesanan ({cartCount} item):</p>
            <h4 style={{ margin: 0, color: '#10b981', fontSize: '16px' }}>Rp {cartTotal.toLocaleString('id-ID')}</h4>
          </div>
          <button 
            onClick={() => navigate('/checkout')}
            style={{ padding: '10px 18px', background: '#e11d48', color: '#fff', border: 'none', borderRadius: '8px', fontWeight: 'bold', cursor: 'pointer' }}
          >
            Check Out →
          </button>
        </div>
      )}
    </div>
  );
}