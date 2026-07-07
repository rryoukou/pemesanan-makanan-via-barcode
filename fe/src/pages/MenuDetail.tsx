import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

interface Product {
  productId: number;
  productName: string;
  category: string;
  price: number;
  description: string;
  imageUrl: string;
}

export default function MenuDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [product, setProduct] = useState<Product | null>(null);
  const [quantity, setQuantity] = useState(1);
  const [notes, setNotes] = useState('');
  const [loading, setLoading] = useState(true);

  // 1. Ambil data detail produk dari API Backend C#
  useEffect(() => {
    const fetchProductDetail = async () => {
      try {
        const response = await fetch(`http://localhost:5029/api/products/${id}`);
        if (response.ok) {
          const data = await response.json();
          setProduct(data);
        }
      } catch (error) {
        console.error("Gagal memuat detail produk:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchProductDetail();
  }, [id]);

  // 2. Fungsi untuk memasukkan pesanan ke Keranjang Belanja (LocalStorage)
  const handleAddToBag = () => {
    if (!product) return;

    // Ambil isi keranjang lama yang sudah ada di localstorage (jika ada)
    const existingCart = JSON.parse(localStorage.getItem('cart') || '[]');

    // Struktur item baru yang mau dimasukkan
    const newItem = {
      productId: product.productId,
      productName: product.productName,
      price: product.price,
      quantity: quantity,
      notes: notes
    };

    // Cek apakah produk dengan catatan yang SAMA sudah pernah dimasukkan?
    // Jika produk sama tapi catatan beda (misal: satu pake cabai, satu enggak), kita pisah barisnya.
    const itemIndex = existingCart.findIndex(
      (item: any) => item.productId === product.productId && item.notes === notes
    );

    if (itemIndex > -1) {
      // Kalau sama persis, cukup tambahkan kuantitasnya
      existingCart[itemIndex].quantity += quantity;
    } else {
      // Kalau baru, push ke dalam array keranjang
      existingCart.push(newItem);
    }

    // Simpan kembali keranjang terbaru ke LocalStorage
    localStorage.setItem('cart', JSON.stringify(existingCart));
    
    alert(`Berhasil menambahkan ${quantity} ${product.productName} ke keranjang!`);
    navigate('/menu'); // Kembalikan customer ke halaman daftar menu utama
  };

  if (loading) return <p>Sedang memuat detail menu...</p>;
  if (!product) return <p>Menu tidak ditemukan.</p>;

  return (
    <div>
      <button onClick={() => navigate('/menu')} style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: '16px', marginBottom: '16px', color: '#6b7280' }}>
        ← Kembali
      </button>

      <img 
        src={product.imageUrl || 'https://placehold.co/400x250?text=Food'} 
        alt={product.productName} 
        style={{ width: '100%', height: '220px', objectFit: 'cover', borderRadius: '12px', marginBottom: '16px' }}
      />

      <h2>{product.productName}</h2>
      <p style={{ color: '#6b7280', fontSize: '14px', lineHeight: '1.5' }}>{product.description}</p>
      <h3 style={{ color: '#10b981', margin: '16px 0' }}>Rp {product.price.toLocaleString('id-ID')}</h3>

      <hr style={{ border: '0', borderTop: '1px solid #e5e7eb', margin: '20px 0' }} />

      {/* Input Notes / Catatan Tambahan */}
      <div style={{ marginBottom: '20px' }}>
        <label style={{ fontWeight: 'bold', display: 'block', marginBottom: '8px', fontSize: '14px' }}>Catatan untuk Menu ini:</label>
        <input 
          type="text" 
          placeholder="Contoh: mie pedas level 3 tanpa daun bawang"
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          style={{ width: '100%', padding: '12px', borderRadius: '8px', border: '1px solid #ccc', boxSizing: 'border-box' }}
        />
      </div>

      {/* Pengatur Kuantitas (Quantity Counter) */}
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '24px' }}>
        <span style={{ fontWeight: 'bold', fontSize: '14px' }}>Atur Jumlah Pemesanan:</span>
        <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
          <button 
            onClick={() => setQuantity(q => Math.max(1, q - 1))}
            style={{ width: '36px', height: '36px', borderRadius: '50%', border: '1px solid #ccc', background: '#fff', cursor: 'pointer', fontSize: '18px' }}
          >
            -
          </button>
          <span style={{ fontWeight: 'bold', fontSize: '16px', minWidth: '20px', textAlign: 'center' }}>{quantity}</span>
          <button 
            onClick={() => setQuantity(q => q + 1)}
            style={{ width: '36px', height: '36px', borderRadius: '50%', border: '1px solid #ccc', background: '#fff', cursor: 'pointer', fontSize: '18px' }}
          >
            +
          </button>
        </div>
      </div>

      {/* Button Add to Bag */}
      <button 
        onClick={handleAddToBag}
        style={{
          width: '100%',
          padding: '14px',
          background: '#e11d48',
          color: '#fff',
          border: 'none',
          borderRadius: '8px',
          fontWeight: 'bold',
          fontSize: '16px',
          cursor: 'pointer'
        }}
      >
        Tambahkan Pesanan
      </button>
    </div>
  );
}