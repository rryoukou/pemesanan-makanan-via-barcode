import { useState, useEffect, useRef } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import QRCode from 'qrcode';

interface OrderDetailResponse {
  orderId: number;
  customerName: string;
  tableNumber: string;
  totalAmount: number;
  status: string;
  itemsBeli: Array<{
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
  const canvasRef = useRef<HTMLCanvasElement>(null);

  useEffect(() => {
    const fetchOrderData = async () => {
      try {
        const response = await fetch(`https://localhost/api/orders?search=${id}`);
        if (response.ok) {
          const data = await response.json();
          if (data && data.length > 0) setOrder(data[0]);
        }
      } catch (error) {
        console.error('Gagal memuat status order:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchOrderData();
  }, [id]);

  // Generate QR code ke canvas setelah order tersedia
  useEffect(() => {
    if (order && canvasRef.current) {
      QRCode.toCanvas(canvasRef.current, `ORD-${String(order.orderId).padStart(5, '0')}`, {
        width: 180,
        margin: 2,
        color: {
          dark: '#1c1917',
          light: '#ffffff',
        },
        errorCorrectionLevel: 'H',
      });
    }
  }, [order]);

  // ── Loading ──
  if (loading) {
    return (
      <div className="min-h-screen bg-[#faf7f4] flex items-center justify-center font-sans">
        <div className="flex flex-col items-center gap-4 animate-pulse">
          <div className="w-20 h-20 bg-amber-200 rounded-full" />
          <div className="h-7 bg-stone-200 rounded-full w-40" />
          <div className="w-full max-w-sm h-96 bg-white rounded-3xl shadow-sm mt-4" />
        </div>
      </div>
    );
  }

  // ── Not found ──
  if (!order) {
    return (
      <div className="min-h-screen bg-[#faf7f4] flex flex-col items-center justify-center font-sans gap-3">
        <p className="text-stone-400 text-sm">Data pesanan tidak ditemukan.</p>
        <button onClick={() => navigate('/menu')} className="text-amber-500 font-semibold text-sm">
          ← Kembali ke Menu
        </button>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-[#faf7f4] font-sans flex flex-col items-center px-4 py-10">

      {/* ── Check icon ── */}
      <div className="relative mb-4">
        <div className="w-20 h-20 rounded-full bg-amber-400 flex items-center justify-center shadow-lg shadow-amber-300/50">
          <svg className="w-10 h-10 text-white" fill="none" stroke="currentColor" strokeWidth="3" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" d="M4.5 12.75l6 6 9-13.5" />
          </svg>
        </div>
        <div className="absolute inset-0 rounded-full bg-amber-300/30 scale-125 animate-ping" />
      </div>

      {/* ── Title ── */}
      <h1 className="text-[32px] font-black text-[#2e2520] tracking-tight">Thank You!</h1>
      <p className="text-stone-400 text-[14px] mt-1 mb-6">Your Order has been received.</p>

      {/* ── Main card ── */}
      <div className="w-full max-w-sm bg-white rounded-3xl shadow-lg shadow-stone-200/70 overflow-hidden">

        {/* Order by header */}
        <div className="flex flex-col items-center pt-7 pb-5 px-6 border-b border-dashed border-stone-200">
          <p className="text-[10px] font-bold text-stone-400 uppercase tracking-[0.15em] mb-1">Order By</p>
          <p className="text-[22px] font-black text-[#2e2520]">{order.customerName}</p>
          <div className="flex items-center gap-3 mt-2">
            <span className="text-[12px] text-stone-400 font-medium bg-stone-100 px-3 py-1 rounded-full">
              Meja {order.tableNumber}
            </span>
            <span className="text-[12px] font-semibold text-amber-600 bg-amber-50 px-3 py-1 rounded-full border border-amber-100">
              #{String(order.orderId).padStart(5, '0')}
            </span>
          </div>
        </div>

        {/* QR code section */}
        <div className="flex flex-col items-center px-6 pt-6 pb-5 border-b border-dashed border-stone-200">
          <p className="text-[13px] text-stone-500 text-center mb-5 leading-relaxed">
            Silahkan menuju kasir untuk melakukan pembayaran.
          </p>

          {/* QR card */}
          <div className="bg-white border border-stone-200 rounded-2xl p-4 shadow-sm flex flex-col items-center gap-2">
            <canvas ref={canvasRef} className="rounded-xl" />
            <div className="flex flex-col items-center mt-1">
              <p className="text-[11px] font-black text-[#2e2520] tracking-[0.2em] uppercase">Scan to Pay</p>
              <p className="text-[9px] text-stone-300 mt-0.5">Powered by FastBite POS</p>
            </div>
          </div>

          <p className="text-[11px] text-stone-400 text-center mt-4 leading-relaxed">
            Tunjukkan QR Code ini ke kasir untuk melakukan pembayaran
          </p>
        </div>

        {/* Item list */}
        <div className="px-6 pt-5 pb-2">
          <div className="space-y-3">
            {order.itemsBeli?.map((item, idx) => (
              <div key={idx} className="flex items-center justify-between gap-3">
                <div className="flex items-center gap-3 flex-1 min-w-0">
                  {/* Qty badge */}
                  <span className="w-8 h-8 bg-amber-50 border border-amber-100 text-amber-600 text-[12px] font-black rounded-lg flex items-center justify-center shrink-0">
                    {item.quantity}x
                  </span>
                  <div className="min-w-0">
                    <p className="text-[13px] font-semibold text-[#2e2520] truncate">{item.namaProduk}</p>
                    {item.notes && (
                      <p className="text-[11px] text-stone-400 italic truncate">✍️ {item.notes}</p>
                    )}
                  </div>
                </div>
                <span className="text-[13px] font-bold text-[#2e2520] whitespace-nowrap shrink-0">
                  Rp{(item.price * item.quantity).toLocaleString('id-ID')}
                </span>
              </div>
            ))}
          </div>
        </div>

        {/* Divider gerigi */}
        <div className="relative flex items-center my-4 px-2">
          <div className="w-5 h-5 bg-[#faf7f4] rounded-full -ml-4 shrink-0 border border-stone-100" />
          <div className="flex-1 border-t-2 border-dashed border-stone-200" />
          <div className="w-5 h-5 bg-[#faf7f4] rounded-full -mr-4 shrink-0 border border-stone-100" />
        </div>

        {/* Total */}
        <div className="flex items-center justify-between px-6 pb-7">
          <span className="text-[15px] font-bold text-[#2e2520]">Total Amount</span>
          <span className="text-[20px] font-black text-amber-600">
            Rp{order.totalAmount.toLocaleString('id-ID')}
          </span>
        </div>
      </div>

      {/* ── Return to Menu button ── */}
      <button
        onClick={() => navigate('/menu')}
        className="mt-6 w-full max-w-sm flex items-center justify-center gap-2.5 bg-amber-400 hover:bg-amber-500 active:bg-amber-600 text-white font-bold py-4 rounded-full shadow-lg shadow-amber-300/40 text-[15px] transition"
      >
        <svg className="w-4 h-4" fill="none" stroke="currentColor" strokeWidth="2.5" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" d="M4.5 10.5L12 3m0 0l7.5 7.5M12 3v18" />
        </svg>
        Return to Menu
      </button>

      <p className="text-[11px] text-stone-400 mt-4 mb-2 text-center">
        Terima kasih telah memesan di FastBite! 🎉
      </p>
    </div>
  );
}
