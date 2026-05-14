"use client";

interface PaginationProps {
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

export default function Pagination({
  page,
  totalPages,
  onPageChange,
}: PaginationProps) {
  return (
    <div className="flex items-center justify-center gap-3">
      <button
        disabled={page === 1}
        onClick={() => onPageChange(Math.max(1, page - 1))}
        className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
      >
        Назад
      </button>

      <span className="text-sm">
        Сторінка {page} з {totalPages}
      </span>

      <button
        disabled={page === totalPages}
        onClick={() => onPageChange(Math.min(totalPages, page + 1))}
        className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
      >
        Вперед
      </button>
    </div>
  );
}