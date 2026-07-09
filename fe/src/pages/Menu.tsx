import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import MenuDetail from './MenuDetail';

interface Product {
  productId: number;
  productName: string;
  category: string;
  price: number;
  description: string;
  imageUrl: string;
  isAvailable: boolean;
}

interface CartItem {
  productId: number;
  productName: string;
  price: number;
  quantity: number;
  imageUrl: string;
}

function getCart(): CartItem[] {
  return JSON.parse(localStorage.getItem('cart') || '[]');
}

function saveCart(cart: CartItem[]) {
  localStorage.setItem('cart', JSON.stringify(cart));
}

const BADGES = [
  { label: 'Terpopuler', color: 'bg-red-500' },
  null,
  { label: 'Promo 10%', color: 'bg-yellow-400 text-stone-800' },
  null,
  { label: 'Baru', color: 'bg-green-500' },
  null,
];

const CATEGORIES = ['All', 'Makanan Berat', 'Makanan Ringan', 'Minuman'];

export default function Menu() {
  const [products, setProducts] = useState<Product[]>([]);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(true);
  const [cart, setCart] = useState<CartItem[]>(getCart());
  const [activeCategory, setActiveCategory] = useState('All');
  const [selectedProductId, setSelectedProductId] = useState<number | null>(null);

  const navigate = useNavigate();
  const nomorMeja = localStorage.getItem('table_number') || '01';

  const cartCount = cart.reduce((sum, i) => sum + i.quantity, 0);
  const cartTotal = cart.reduce((sum, i) => sum + i.price * i.quantity, 0);

  useEffect(() => { saveCart(cart); }, [cart]);

  const handleCloseModal = () => {
    setSelectedProductId(null);
    setCart(getCart());
  };

  const fetchProducts = async (keyword: string) => {
    try {
      setLoading(true);
      const url = keyword
        ? `https://localhost/api/products?search=${keyword}`
        : `https://localhost/api/products`;
      const res = await fetch(url, {
        headers: {
          'X-Tunnel-Skip-Anti-Phishing-Page': 'true',
        },
      });
      setProducts(await res.json());
    } catch (e) {
      console.error('Gagal mengambil data menu:', e);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const t = setTimeout(() => fetchProducts(search), 500);
    return () => clearTimeout(t);
  }, [search]);

  const getItemQty = (id: number) =>
    cart.find((i) => i.productId === id)?.quantity ?? 0;

  const addToCart = (e: React.MouseEvent, item: Product) => {
    e.stopPropagation();
    setCart((prev) => {
      const ex = prev.find((i) => i.productId === item.productId);
      if (ex) return prev.map((i) => i.productId === item.productId ? { ...i, quantity: i.quantity + 1 } : i);
      return [...prev, { productId: item.productId, productName: item.productName, price: item.price, quantity: 1, imageUrl: item.imageUrl }];
    });
  };

  const decreaseFromCart = (e: React.MouseEvent, productId: number) => {
    e.stopPropagation();
    setCart((prev) => {
      const ex = prev.find((i) => i.productId === productId);
      if (!ex) return prev;
      if (ex.quantity <= 1) return prev.filter((i) => i.productId !== productId);
      return prev.map((i) => i.productId === productId ? { ...i, quantity: i.quantity - 1 } : i);
    });
  };

  return (
    <>
      {/* ── Outer: full-width di semua layar, bg cream ── */}
      <div className="min-h-screen bg-[#faf9f7]">
        <div className="w-full min-h-screen pb-28">

          {/* ── HEADER ── */}
          <header className="sticky top-0 z-30 bg-[#faf9f7] px-4 md:px-8 lg:px-16 pt-4 pb-3">
            <div className="max-w-screen-xl mx-auto">
            <div className="flex items-center justify-between mb-3">

              {/* Logo kiri */}
              <div className="flex items-center gap-2">
                {/* Logo icon — fork & spoon dalam lingkaran kuning */}
                <div className="w-9 h-9 rounded-xl bg-[#2e2520] flex items-center justify-center shadow-sm shrink-0">
                  <svg className="w-5 h-5 text-amber-400" viewBox="0 0 24 24" fill="currentColor">
                    <path d="M11 2a1 1 0 00-1 1v4.586l-1.293-1.293a1 1 0 00-1.414 1.414L9 9.414V21a1 1 0 002 0V9.414l1.707-1.707a1 1 0 00-1.414-1.414L10 7.586V3a1 1 0 00-1-1z"/>
                    <path d="M15 2a1 1 0 00-1 1v5c0 1.306.835 2.418 2 2.83V21a1 1 0 002 0V10.83A3.001 3.001 0 0020 8V3a1 1 0 00-2 0v4h-1V3a1 1 0 00-2 0v4h-1V3a1 1 0 00-1-1z"/>
                  </svg>
                </div>
                <div className="leading-tight">
                  <p className="text-[15px] font-black text-[#2e2520] tracking-tight leading-none">FastBite</p>
                  <p className="text-[10px] text-stone-400 font-medium">Meja No: <span className="text-amber-500 font-bold">{nomorMeja}</span></p>
                </div>
              </div>

              {/* Cart button kanan */}
              <button
                onClick={() => navigate('/checkout')}
                className="relative w-10 h-10 flex items-center justify-center rounded-xl bg-[#2e2520] shadow-sm"
              >
                <svg className="w-5 h-5 text-white" fill="none" stroke="currentColor" strokeWidth="2" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 00-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 00-16.536-1.84M7.5 14.25L5.106 5.272M6 20.25a.75.75 0 11-1.5 0 .75.75 0 011.5 0zm12.75 0a.75.75 0 11-1.5 0 .75.75 0 011.5 0z" />
                </svg>
                {cartCount > 0 && (
                  <span className="absolute -top-1.5 -right-1.5 w-5 h-5 bg-amber-400 text-white text-[10px] font-black rounded-full flex items-center justify-center leading-none">
                    {cartCount > 9 ? '9+' : cartCount}
                  </span>
                )}
              </button>
            </div>

            {/* Search bar */}
            <div className="relative">
              <svg className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-stone-400" fill="none" stroke="currentColor" strokeWidth="2.2" viewBox="0 0 24 24">
                <circle cx="11" cy="11" r="8" />
                <path d="M21 21l-4.35-4.35" strokeLinecap="round" />
              </svg>
              <input
                type="text"
                placeholder="Cari menu favoritmu..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                className="w-full pl-10 pr-4 py-3 rounded-2xl bg-white border border-stone-200 text-[14px] text-[#2e2520] placeholder-stone-400 outline-none focus:border-amber-400 focus:ring-2 focus:ring-amber-100 transition"
              />
            </div>

            {/* Category tabs */}
            <div className="flex gap-2 mt-3 overflow-x-auto scrollbar-hide pb-0.5">
              {CATEGORIES.map((cat) => (
                <button
                  key={cat}
                  onClick={() => setActiveCategory(cat)}
                  className={`flex-shrink-0 px-4 py-1.5 rounded-full text-[13px] font-semibold transition ${
                    activeCategory === cat
                      ? 'bg-amber-400 text-white shadow-sm'
                      : 'bg-white text-stone-500 border border-stone-200'
                  }`}
                >
                  {cat}
                </button>
              ))}
            </div>
            </div>{/* end max-w header */}
          </header>
          {/* ── PRODUCT LIST ── */}
          <main className="px-4 md:px-8 lg:px-16 pt-2">
            <div className="max-w-screen-xl mx-auto">

            {/* Skeleton */}
            {loading && (
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 md:gap-6">
                {[1, 2, 3, 4, 5, 6, 7, 8].map((n) => (
                  <div key={n} className="bg-white rounded-2xl overflow-hidden animate-pulse">
                    <div className="w-full h-48 bg-stone-200" />
                    <div className="p-3 space-y-2">
                      <div className="h-4 bg-stone-200 rounded w-2/3" />
                      <div className="h-3 bg-stone-200 rounded w-full" />
                      <div className="h-10 bg-stone-200 rounded-xl mt-2" />
                    </div>
                  </div>
                ))}
              </div>
            )}

            {/* Empty */}
            {!loading && products.length === 0 && (
              <div className="flex flex-col items-center justify-center mt-16 text-stone-400">
                <svg className="w-14 h-14 mb-3" fill="none" stroke="currentColor" strokeWidth="1.5" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" d="M12 9v3.75m9-.75a9 9 0 11-18 0 9 9 0 0118 0zm-9 3.75h.008v.008H12v-.008z" />
                </svg>
                <p className="text-[14px] font-medium">Menu tidak ditemukan</p>
                <p className="text-[12px] mt-1">Coba kata kunci lain atau cek koneksi</p>
              </div>
            )}

            {/* Cards — 1 kolom di HP, grid di tablet/laptop */}
            {!loading && products.length > 0 && (
              <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 md:gap-6">
                {products.filter(item => activeCategory === 'All' || item.category === activeCategory).map((item, idx) => {
                  const qty = getItemQty(item.productId);
                  const badge = BADGES[idx % BADGES.length];

                  return (
                    <div
                      key={item.productId}
                      onClick={() => setSelectedProductId(item.productId)}
                      className="bg-white rounded-2xl overflow-hidden shadow-sm cursor-pointer active:scale-[0.98] transition-transform"
                    >
                      {/* Gambar full-width */}
                      <div className="relative w-full h-48">
                        <img
                          src={item.imageUrl || 'https://placehold.co/400x200/f5e6d3/a16207?text=Food'}
                          alt={item.productName}
                          className="w-full h-full object-cover"
                        />
                        {badge && (
                          <span className={`absolute top-2.5 left-2.5 text-[11px] font-bold text-white px-2.5 py-1 rounded-full ${badge.color}`}>
                            {badge.label}
                          </span>
                        )}
                      </div>

                      {/* Body */}
                      <div className="px-3 pt-2.5 pb-3">
                        {/* Nama + harga */}
                        <div className="flex items-start justify-between gap-2">
                          <h3 className="text-[15px] font-bold text-[#2e2520] leading-tight flex-1">
                            {item.productName}
                          </h3>
                          <span className="text-[14px] font-bold text-amber-500 whitespace-nowrap">
                            Rp {item.price.toLocaleString('id-ID')}
                          </span>
                        </div>

                        {/* Deskripsi */}
                        <p className="text-[12px] text-stone-400 mt-1 line-clamp-2 leading-relaxed">
                          {item.description}
                        </p>

                        {/* Tombol Add / Counter */}
                        <div className="mt-3" onClick={(e) => e.stopPropagation()}>
                          {qty === 0 ? (
                            <button
                              onClick={(e) => addToCart(e, item)}
                              className="w-full py-2.5 rounded-full bg-amber-400 hover:bg-amber-500 active:bg-amber-600 text-white font-semibold text-[14px] transition"
                            >
                              + Add
                            </button>
                          ) : (
                            <div className="flex items-center justify-between bg-[#2e2520] rounded-full px-2 py-1">
                              <button
                                onClick={(e) => decreaseFromCart(e, item.productId)}
                                className="w-9 h-9 flex items-center justify-center text-white text-xl font-bold rounded-full hover:bg-white/10 transition"
                              >
                                −
                              </button>
                              <span className="text-white font-bold text-[15px] min-w-[24px] text-center">
                                {qty}
                              </span>
                              <button
                                onClick={(e) => addToCart(e, item)}
                                className="w-9 h-9 flex items-center justify-center text-white text-xl font-bold rounded-full hover:bg-white/10 transition"
                              >
                                +
                              </button>
                            </div>
                          )}
                        </div>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
            </div>{/* end max-w-screen-xl */}
          </main>

          {/* ── FLOATING CHECKOUT BAR ── */}
          {cartCount > 0 && (
            <div className="fixed bottom-4 left-0 right-0 flex justify-center z-40 px-4">
              <div className="bg-[#2e2520] rounded-2xl px-4 py-3 flex items-center justify-between shadow-xl w-full max-w-sm">
                <div className="flex items-center gap-3">
                  <div className="relative">
                    <div className="w-10 h-10 bg-white/10 rounded-xl flex items-center justify-center">
                      <svg className="w-5 h-5 text-white" fill="none" stroke="currentColor" strokeWidth="2" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 00-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 00-16.536-1.84M7.5 14.25L5.106 5.272M6 20.25a.75.75 0 11-1.5 0 .75.75 0 011.5 0zm12.75 0a.75.75 0 11-1.5 0 .75.75 0 011.5 0z" />
                      </svg>
                    </div>
                    <span className="absolute -top-1.5 -right-1.5 w-5 h-5 bg-amber-400 text-white text-[10px] font-bold rounded-full flex items-center justify-center">
                      {cartCount}
                    </span>
                  </div>
                  <div>
                    <p className="text-[10px] text-white/50 leading-none mb-0.5">Total Pesanan</p>
                    <p className="text-[15px] font-bold text-white leading-none">
                      Rp {cartTotal.toLocaleString('id-ID')}
                    </p>
                  </div>
                </div>

                <button
                  onClick={() => navigate('/checkout')}
                  className="bg-amber-400 hover:bg-amber-500 active:bg-amber-600 text-white font-bold text-[13px] px-5 py-2.5 rounded-xl transition"
                >
                  Check Out →
                </button>
              </div>
            </div>
          )}

        </div>
      </div>

      {/* ── MODAL MENU DETAIL ── */}
      {selectedProductId !== null && (
        <MenuDetail
          id={selectedProductId}
          onClose={handleCloseModal}
        />
      )}
    </>
  );
}
