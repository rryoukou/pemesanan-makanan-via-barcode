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

export default function AdminInventory() {
  const [products, setProducts] = useState<Product[]>([]);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  // State Formulir Tambah/Edit Produk
  const [isEditing, setIsEditing] = useState(false);
  const [currentId, setCurrentId] = useState<number | null>(null);
  const [productName, setProductName] = useState('');
  const [category, setCategory] = useState('Makanan Berat'); // Default kategori awal
  const [price, setPrice] = useState(0);
  const [description, setDescription] = useState('');
  const [imageUrl, setImageUrl] = useState('');
  const [isAvailable, setIsAvailable] = useState(true);

  const fetchInventory = async (keyword: string) => {
    try {
      setLoading(true);
      const url = keyword ? `http://localhost:5029/api/products?search=${keyword}` : `http://localhost:5029/api/products`;
      const response = await fetch(url);
      const data = await response.json();
      setProducts(data);
    } catch (error) {
      console.error("Gagal memuat inventori produk:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchInventory(search);
  }, [search]);

  // Handle Operasi Simpan (Tambah Baru ATAU Edit Lama)
  const handleSaveProduct = async (e: React.FormEvent) => {
    e.preventDefault();
    
    const productPayload = {
      productId: isEditing ? currentId : 0,
      productName,
      category,
      price: Number(price),
      description,
      imageUrl,
      isAvailable
    };

    const url = isEditing ? `http://localhost:5029/api/products/${currentId}` : `http://localhost:5029/api/products`;
    const method = isEditing ? 'PUT' : 'POST';

    try {
      const response = await fetch(url, {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(productPayload)
      });

      if (response.ok) {
        alert(isEditing ? "Produk berhasil diperbarui!" : "Produk baru berhasil ditambahkan!");
        resetForm();
        fetchInventory(search);
      }
    } catch (error) {
      console.error("Gagal memproses produk:", error);
    }
  };

  // Tombol Trigger Aksi Edit
  const handleEditClick = (product: Product) => {
    setIsEditing(true);
    setCurrentId(product.productId);
    setProductName(product.productName);
    setCategory(product.category);
    setPrice(product.price);
    setDescription(product.description);
    setImageUrl(product.imageUrl);
    setIsAvailable(product.isAvailable);
  };

  // Tombol Aksi Hapus Produk
  const handleDeleteClick = async (productId: number) => {
    if (!window.confirm("Apakah kamu yakin ingin menghapus produk menu ini?")) return;
    try {
      const response = await fetch(`http://localhost:5029/api/products/${productId}`, { method: 'DELETE' });
      if (response.ok) {
        alert("Produk berhasil dihapus dari menu.");
        fetchInventory(search);
      }
    } catch (error) {
      console.error("Gagal menghapus produk:", error);
    }
  };

  const resetForm = () => {
    setIsEditing(false);
    setCurrentId(null);
    setProductName('');
    setCategory('Makanan Berat');
    setPrice(0);
    setDescription('');
    setImageUrl('');
    setIsAvailable(true);
  };

  return (
    <div style={{ fontFamily: 'sans-serif', color: '#1e293b' }}>
      {/* NAVIGATION BAR ADMIN */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', borderBottom: '2px solid #e2e8f0', paddingBottom: '16px', marginBottom: '24px' }}>
        <h2 style={{ margin: 0, color: '#e11d48' }}>Inventory Gudang Produk</h2>
        <div style={{ display: 'flex', gap: '12px' }}>
          <button onClick={() => navigate('/admin/dashboard')} style={{ padding: '8px 16px', background: '#f1f5f9', border: '1px solid #cbd5e1', borderRadius: '6px', cursor: 'pointer' }}>Dashboard</button>
          <button onClick={() => navigate('/admin/orders')} style={{ padding: '8px 16px', background: '#f1f5f9', border: '1px solid #cbd5e1', borderRadius: '6px', cursor: 'pointer' }}>Orders</button>
          <button onClick={() => navigate('/admin/inventory')} style={{ padding: '8px 16px', background: '#e11d48', color: '#fff', border: 'none', borderRadius: '6px', fontWeight: 'bold', cursor: 'pointer' }}>Inventory</button>
          <button onClick={() => navigate('/menu')} style={{ padding: '8px 16px', background: '#334155', color: '#fff', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>Keluar</button>
        </div>
      </div>

      {/* FORMULIR CRUD INPUT DATA PRODUK */}
      <div style={{ background: '#f8fafc', border: '1px solid #e2e8f0', padding: '20px', borderRadius: '12px', marginBottom: '32px' }}>
        <h3 style={{ margin: '0 0 16px 0' }}>{isEditing ? "📝 Edit Data Produk Menu" : "➕ Tambah Menu Produk Baru"}</h3>
        <form onSubmit={handleSaveProduct} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '16px' }}>
          <div>
            <label style={{ fontSize: '13px', fontWeight: 'bold', display: 'block', marginBottom: '4px' }}>Nama Produk *</label>
            <input type="text" required value={productName} onChange={(e) => setProductName(e.target.value)} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box' }} />
          </div>
          <div>
            <label style={{ fontSize: '13px', fontWeight: 'bold', display: 'block', marginBottom: '4px' }}>Kategori Produk *</label>
            <select value={category} onChange={(e) => setCategory(e.target.value)} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box' }}>
              <option value="Makanan Ringan">Makanan Ringan</option>
              <option value="Makanan Berat">Makanan Berat</option>
              <option value="Minuman">Minuman</option>
            </select>
          </div>
          <div>
            <label style={{ fontSize: '13px', fontWeight: 'bold', display: 'block', marginBottom: '4px' }}>Harga Menu (Rp) *</label>
            <input type="number" required value={price} onChange={(e) => setPrice(Number(e.target.value))} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box' }} />
          </div>
          <div>
            <label style={{ fontSize: '13px', fontWeight: 'bold', display: 'block', marginBottom: '4px' }}>URL Gambar Produk</label>
            <input type="text" value={imageUrl} onChange={(e) => setImageUrl(e.target.value)} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box' }} />
          </div>
          <div style={{ gridColumn: 'span 2' }}>
            <label style={{ fontSize: '13px', fontWeight: 'bold', display: 'block', marginBottom: '4px' }}>Deskripsi Produk</label>
            <textarea rows={2} value={description} onChange={(e) => setDescription(e.target.value)} style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc', boxSizing: 'border-box', fontFamily: 'sans-serif' }} />
          </div>
          <div style={{ gridColumn: 'span 2', display: 'flex', gap: '16px', alignItems: 'center' }}>
            <label style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '14px', fontWeight: 'bold', cursor: 'pointer' }}>
              <input type="checkbox" checked={isAvailable} onChange={(e) => setIsAvailable(e.target.checked)} /> Ketersediaan Produk (Ready / Tersedia)
            </label>
          </div>
          <div style={{ gridColumn: 'span 2', display: 'flex', gap: '8px', marginTop: '8px' }}>
            <button type="submit" style={{ padding: '10px 20px', background: '#10b981', color: '#fff', border: 'none', borderRadius: '6px', fontWeight: 'bold', cursor: 'pointer' }}>{isEditing ? "Simpan Perubahan" : "Tambah ke Menu"}</button>
            {isEditing && <button type="button" onClick={resetForm} style={{ padding: '10px 20px', background: '#64748b', color: '#fff', border: 'none', borderRadius: '6px', cursor: 'pointer' }}>Batal Edit</button>}
          </div>
        </form>
      </div>

      {/* INPUT FILTER DATA INVENTORY */}
      <input type="text" placeholder="Cari nama produk dari gudang..." value={search} onChange={(e) => setSearch(e.target.value)} style={{ width: '100%', padding: '12px', borderRadius: '8px', border: '1px solid #ccc', marginBottom: '20px', boxSizing: 'border-box' }} />

      {/* TABEL LIST INVENTORY */}
      {loading ? (
        <p>Sedang menyinkronkan data gudang...</p>
      ) : (
        <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left', fontSize: '14px' }}>
          <thead>
            <tr style={{ borderBottom: '2px solid #e2e8f0', background: '#f8fafc' }}>
              <th style={{ padding: '12px' }}>Foto & Nama Produk</th>
              <th style={{ padding: '12px' }}>Category</th>
              <th style={{ padding: '12px' }}>Harga</th>
              <th style={{ padding: '12px' }}>Status Stok</th>
              <th style={{ padding: '12px' }}>Action</th>
            </tr>
          </thead>
          <tbody>
            {products.map((item) => (
              <tr key={item.productId} style={{ borderBottom: '1px solid #f1f5f9' }}>
                <td style={{ padding: '12px', display: 'flex', alignItems: 'center', gap: '12px' }}>
                  <img src={item.imageUrl || 'https://placehold.co/50x50?text=Food'} alt={item.productName} style={{ width: '45px', height: '45px', objectFit: 'cover', borderRadius: '6px' }} />
                  <span style={{ fontWeight: 'bold' }}>{item.productName}</span>
                </td>
                <td style={{ padding: '12px' }}>{item.category}</td>
                <td style={{ padding: '12px', fontWeight: 'bold', color: '#10b981' }}>Rp {item.price.toLocaleString('id-ID')}</td>
                <td style={{ padding: '12px' }}>
                  <span style={{ padding: '4px 8px', borderRadius: '4px', fontSize: '11px', fontWeight: 'bold', backgroundColor: item.isAvailable ? '#dcfce7' : '#fee2e2', color: item.isAvailable ? '#15803d' : '#b91c1c' }}>
                    {item.isAvailable ? "Tersedia" : "Habis"}
                  </span>
                </td>
                <td style={{ padding: '12px' }}>
                  <div style={{ display: 'flex', gap: '8px' }}>
                    <button onClick={() => handleEditClick(item)} style={{ padding: '4px 8px', background: '#f59e0b', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '12px' }}>Edit</button>
                    <button onClick={() => handleDeleteClick(item.productId)} style={{ padding: '4px 8px', background: '#e11d48', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', fontSize: '12px' }}>Hapus</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}